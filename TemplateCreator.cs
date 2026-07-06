using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PrePrintPairingLabel
{
    internal static class TemplateCreator
    {
        private const string PROGID        = "BarTender.Application";
        private const short  BT_BARCODE    = 4;   // BtObjectType.btObjectBarcode
        private const string SCREEN_DATA   = "0"; // BtSubStringType.btSubStringScreenData
        private const string FIELD_NAME    = "Barcode";
        private const string DATAMATRIX    = "Data Matrix ECC 200";

        /// <summary>
        /// Creates a BarTender .btw template with one DataMatrix barcode sourced
        /// from a Named SubString called "Barcode".  The Print Engine SDK sets
        /// that SubString value before each print call.
        /// </summary>
        public static void Create(string templatePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(templatePath)));

            Type appType = Type.GetTypeFromProgID(PROGID, throwOnError: true);
            dynamic btApp  = null;
            dynamic format = null;

            try
            {
                btApp         = Activator.CreateInstance(appType);
                btApp.Visible = false;

                format = btApp.Formats.Add();

                dynamic bc = format.Objects.Create(BT_BARCODE);

                // Set symbology to DataMatrix ECC 200
                bc.SetProperty("Symbology", DATAMATRIX);

                // Link the barcode data to a Screen Data (SubString) field named "Barcode".
                // This makes format.SubStrings["Barcode"] accessible via the Print Engine SDK.
                bc.SetProperty("SubStringType", SCREEN_DATA);
                bc.SetProperty("SubStringName", FIELD_NAME);

                format.SaveAs(templatePath);
            }
            finally
            {
                if (format != null) { try { format.Close(false); } catch { } Marshal.ReleaseComObject(format); }
                if (btApp  != null) { try { btApp.Quit(false);   } catch { } Marshal.ReleaseComObject(btApp); }
            }
        }
    }
}
