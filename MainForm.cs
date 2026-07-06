using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using Seagull.BarTender.Print;

namespace PrePrintPairingLabel
{
    public partial class MainForm : Form
    {
        private Engine _engine;
        private LabelFormatDocument _format;
        private List<LabelTypeConfig> _labelTypes;
        private bool _suppressTypeChange;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetStatus("Starting BarTender engine...");
            _labelTypes = LoadLabelTypeConfigs();

            var worker = new BackgroundWorker();
            worker.DoWork += (s, args) =>
            {
                try { _engine = new Engine(true); }
                catch (PrintEngineException ex) { args.Result = ex.Message; }
            };
            worker.RunWorkerCompleted += (s, args) =>
            {
                if (args.Result is string err)
                {
                    MessageBox.Show(this, "Failed to start BarTender engine:\n" + err, Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                tsslEngine.Text = "Engine: Ready";
                PopulateLabelTypes();
                nudQuantity.Enabled = true;
            };
            worker.RunWorkerAsync();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_format != null) { try { _format.Close(SaveOptions.DoNotSaveChanges); } catch { } }
            if (_engine != null) { try { _engine.Stop(SaveOptions.DoNotSaveChanges); } catch { } }
        }

        private void cboLabelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressTypeChange || cboLabelType.SelectedIndex < 0) return;
            LoadSelectedTemplate();
        }

        private void btnTestPrint_Click(object sender, EventArgs e) => RunPrintJob(isTest: true);

        private void btnPrint_Click(object sender, EventArgs e) => RunPrintJob(isTest: false);

        private void PopulateLabelTypes()
        {
            _suppressTypeChange = true;
            cboLabelType.Items.Clear();
            foreach (var lt in _labelTypes)
                cboLabelType.Items.Add(lt.Name);
            _suppressTypeChange = false;

            if (_labelTypes.Count == 0)
            {
                SetStatus("No label types configured — edit app.config.");
                return;
            }

            cboLabelType.Enabled = true;
            cboLabelType.SelectedIndex = 0;
        }

        private void LoadSelectedTemplate()
        {
            if (cboLabelType.SelectedIndex < 0 || cboLabelType.SelectedIndex >= _labelTypes.Count)
                return;

            var cfg = _labelTypes[cboLabelType.SelectedIndex];

            if (string.IsNullOrEmpty(cfg.TemplatePath))
            {
                SetStatus($"No template path configured for '{cfg.Name}' — edit app.config.");
                btnTestPrint.Enabled = false;
                btnPrint.Enabled = false;
                return;
            }

            if (!System.IO.File.Exists(cfg.TemplatePath))
            {
                SetStatus($"Template not found: {cfg.TemplatePath}");
                btnTestPrint.Enabled = false;
                btnPrint.Enabled = false;
                return;
            }

            cboLabelType.Enabled = false;
            btnTestPrint.Enabled = false;
            btnPrint.Enabled = false;
            SetStatus($"Loading {cfg.Name} template...");

            var worker = new BackgroundWorker();
            worker.DoWork += (s, args) =>
            {
                try
                {
                    if (_format != null) { _format.Close(SaveOptions.DoNotSaveChanges); _format = null; }
                    _format = _engine.Documents.Open((string)args.Argument);
                }
                catch (Exception ex) { args.Result = ex.Message; }
            };
            worker.RunWorkerCompleted += (s, args) =>
            {
                cboLabelType.Enabled = true;

                if (args.Result is string err)
                {
                    SetStatus($"Failed to load template for '{cfg.Name}'.");
                    MessageBox.Show(this, "Cannot open template:\n\n" + err, Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnTestPrint.Enabled = true;
                btnPrint.Enabled = true;
                UpdatePreview();
                SetStatus($"Ready — {cfg.Name}");
            };
            worker.RunWorkerAsync(cfg.TemplatePath);
        }

        private void UpdatePreview()
        {
            txtPreview.Text = "#PP" + Guid.NewGuid().ToString("N").ToUpper() + "#";
        }

        private void RunPrintJob(bool isTest)
        {
            if (_format == null || cboLabelType.SelectedIndex < 0) return;

            var cfg = _labelTypes[cboLabelType.SelectedIndex];
            int quantity = isTest ? 1 : (int)nudQuantity.Value;

            SetPrintingState(true);
            SetStatus(isTest ? "Sending test label..." : $"Printing {quantity} label(s)...");

            var worker = new BackgroundWorker { WorkerReportsProgress = true };

            worker.DoWork += (s, args) =>
            {
                var bg = (BackgroundWorker)s;
                var ctx = (PrintContext)args.Argument;

                _format.PrintSetup.IdenticalCopiesOfLabel = 1;
                if (_format.PrintSetup.SupportsSerializedLabels)
                    _format.PrintSetup.NumberOfSerializedLabels = 1;

                for (int i = 0; i < ctx.Quantity; i++)
                {
                    string barcodeData = ctx.IsTest
                        ? "#PP00000000000000000000000000000000#"
                        : "#PP" + Guid.NewGuid().ToString("N").ToUpper() + "#";

                    if (!string.IsNullOrEmpty(ctx.BarcodeField) && _format.SubStrings.Count > 0)
                        _format.SubStrings[ctx.BarcodeField].Value = barcodeData;

                    Messages msgs;
                    Result r = _format.Print("PrePrintPairingLabel", 30000, out msgs);

                    if (r == Result.Failure)
                    {
                        args.Result = new PrintJobResult(i, false, FormatMessages(msgs));
                        return;
                    }

                    bg.ReportProgress(0, i + 1);
                }

                args.Result = new PrintJobResult(ctx.Quantity, true, null);
            };

            worker.ProgressChanged += (s, args) =>
                SetStatus($"Printing... {args.UserState}/{quantity}");

            worker.RunWorkerCompleted += (s, args) =>
            {
                SetPrintingState(false);
                UpdatePreview();

                if (args.Error != null)
                {
                    SetStatus("Print failed.");
                    MessageBox.Show(this, args.Error.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var res = (PrintJobResult)args.Result;
                if (res.Success)
                    SetStatus(isTest ? "Test label sent to printer." : $"Printed {res.Printed} label(s) successfully.");
                else
                {
                    SetStatus($"Print failed after {res.Printed} label(s).");
                    MessageBox.Show(this, "Print failed:\n\n" + (res.ErrorMessage ?? "Unknown error"), Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            worker.RunWorkerAsync(new PrintContext(quantity, cfg.BarcodeField, isTest));
        }

        private void SetPrintingState(bool printing)
        {
            cboLabelType.Enabled = !printing;
            nudQuantity.Enabled = !printing;
            btnTestPrint.Enabled = !printing && _format != null;
            btnPrint.Enabled = !printing && _format != null;
        }

        private void SetStatus(string msg)
        {
            if (InvokeRequired) Invoke((Action<string>)SetStatus, msg);
            else tsslStatus.Text = msg;
        }

        private static List<LabelTypeConfig> LoadLabelTypeConfigs()
        {
            var list = new List<LabelTypeConfig>();
            for (int i = 0; ; i++)
            {
                string name = ConfigurationManager.AppSettings["LabelType." + i + ".Name"];
                if (string.IsNullOrEmpty(name)) break;
                list.Add(new LabelTypeConfig
                {
                    Name         = name,
                    TemplatePath = ConfigurationManager.AppSettings["LabelType." + i + ".TemplatePath"] ?? "",
                    BarcodeField = ConfigurationManager.AppSettings["LabelType." + i + ".BarcodeField"] ?? "Barcode"
                });
            }
            return list;
        }

        private static string FormatMessages(Messages messages)
        {
            if (messages == null) return string.Empty;
            var sb = new StringBuilder();
            foreach (Seagull.BarTender.Print.Message m in messages)
                sb.AppendLine(m.Text);
            return sb.ToString().Trim();
        }

        private sealed class LabelTypeConfig
        {
            public string Name { get; set; }
            public string TemplatePath { get; set; }
            public string BarcodeField { get; set; }
        }

        private sealed class PrintContext
        {
            public int Quantity { get; }
            public string BarcodeField { get; }
            public bool IsTest { get; }
            public PrintContext(int qty, string field, bool isTest)
            { Quantity = qty; BarcodeField = field; IsTest = isTest; }
        }

        private sealed class PrintJobResult
        {
            public int Printed { get; }
            public bool Success { get; }
            public string ErrorMessage { get; }
            public PrintJobResult(int printed, bool success, string error)
            { Printed = printed; Success = success; ErrorMessage = error; }
        }
    }
}
