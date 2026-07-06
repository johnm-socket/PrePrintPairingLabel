namespace PrePrintPairingLabel
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblLabelType;
        private System.Windows.Forms.ComboBox cboLabelType;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.Label lblPrinter;
        private System.Windows.Forms.ComboBox cboPrinter;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.TextBox txtPreview;
        private System.Windows.Forms.Button btnTestPrint;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslEngine;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblLabelType = new System.Windows.Forms.Label();
            this.cboLabelType = new System.Windows.Forms.ComboBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblPrinter = new System.Windows.Forms.Label();
            this.cboPrinter = new System.Windows.Forms.ComboBox();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.txtPreview = new System.Windows.Forms.TextBox();
            this.btnTestPrint = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslEngine = new System.Windows.Forms.ToolStripStatusLabel();

            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            this.grpPreview.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // lblLabelType
            this.lblLabelType.AutoSize = true;
            this.lblLabelType.Location = new System.Drawing.Point(12, 22);
            this.lblLabelType.Name = "lblLabelType";
            this.lblLabelType.Size = new System.Drawing.Size(65, 13);
            this.lblLabelType.TabIndex = 0;
            this.lblLabelType.Text = "Label Type:";

            // cboLabelType
            this.cboLabelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLabelType.Enabled = false;
            this.cboLabelType.FormattingEnabled = true;
            this.cboLabelType.Location = new System.Drawing.Point(90, 18);
            this.cboLabelType.Name = "cboLabelType";
            this.cboLabelType.Size = new System.Drawing.Size(338, 21);
            this.cboLabelType.TabIndex = 1;
            this.cboLabelType.SelectedIndexChanged += new System.EventHandler(this.cboLabelType_SelectedIndexChanged);

            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(12, 58);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(52, 13);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Quantity:";

            // nudQuantity
            this.nudQuantity.Enabled = false;
            this.nudQuantity.Location = new System.Drawing.Point(90, 54);
            this.nudQuantity.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            this.nudQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(80, 20);
            this.nudQuantity.TabIndex = 3;
            this.nudQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblPrinter
            this.lblPrinter.AutoSize = true;
            this.lblPrinter.Location = new System.Drawing.Point(12, 94);
            this.lblPrinter.Name = "lblPrinter";
            this.lblPrinter.Size = new System.Drawing.Size(43, 13);
            this.lblPrinter.TabIndex = 4;
            this.lblPrinter.Text = "Printer:";

            // cboPrinter
            this.cboPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrinter.FormattingEnabled = true;
            this.cboPrinter.Location = new System.Drawing.Point(90, 90);
            this.cboPrinter.Name = "cboPrinter";
            this.cboPrinter.Size = new System.Drawing.Size(338, 21);
            this.cboPrinter.TabIndex = 5;

            // grpPreview
            this.grpPreview.Controls.Add(this.txtPreview);
            this.grpPreview.Location = new System.Drawing.Point(12, 126);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(416, 52);
            this.grpPreview.TabIndex = 6;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Next Barcode Preview";

            // txtPreview
            this.txtPreview.BackColor = System.Drawing.SystemColors.Control;
            this.txtPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPreview.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPreview.Location = new System.Drawing.Point(9, 22);
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ReadOnly = true;
            this.txtPreview.Size = new System.Drawing.Size(398, 19);
            this.txtPreview.TabIndex = 0;
            this.txtPreview.TabStop = false;
            this.txtPreview.Text = "#PP00000000000000000000000000000000#";

            // btnTestPrint
            this.btnTestPrint.Enabled = false;
            this.btnTestPrint.Location = new System.Drawing.Point(12, 194);
            this.btnTestPrint.Name = "btnTestPrint";
            this.btnTestPrint.Size = new System.Drawing.Size(135, 36);
            this.btnTestPrint.TabIndex = 7;
            this.btnTestPrint.Text = "Test Print";
            this.btnTestPrint.UseVisualStyleBackColor = true;
            this.btnTestPrint.Click += new System.EventHandler(this.btnTestPrint_Click);

            // btnPrint
            this.btnPrint.Enabled = false;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(293, 194);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(135, 36);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Print Labels";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);

            // statusStrip
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsslStatus,
                this.tsslEngine });
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(440, 22);
            this.statusStrip.TabIndex = 9;

            // tsslStatus
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Spring = true;
            this.tsslStatus.Text = "Initializing...";
            this.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // tsslEngine
            this.tsslEngine.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(
                System.Windows.Forms.ToolStripStatusLabelBorderSides.Left));
            this.tsslEngine.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsslEngine.Name = "tsslEngine";
            this.tsslEngine.Text = "Engine: Starting...";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 260);
            this.Controls.Add(this.lblLabelType);
            this.Controls.Add(this.cboLabelType);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.nudQuantity);
            this.Controls.Add(this.lblPrinter);
            this.Controls.Add(this.cboPrinter);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.btnTestPrint);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrePrint Pairing Label";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);

            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            this.grpPreview.ResumeLayout(false);
            this.grpPreview.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
