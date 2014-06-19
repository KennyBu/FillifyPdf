using System;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using Color = iTextSharp.text.Color;
using Font = iTextSharp.text.Font;

namespace FillifyPdfConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileBase = "Docs/MerchandiseReturnLabelInstructions";
            const string fileExtension = ".pdf";
            
            var pdfTemplate = string.Concat(AppDomain.CurrentDomain.BaseDirectory, fileBase, fileExtension);
            var newFile = string.Concat(AppDomain.CurrentDomain.BaseDirectory, fileBase, Guid.NewGuid().ToString("N"),fileExtension);

            //ParseTheForm(pdfTemplate);
            WriteToFormFields(pdfTemplate, newFile);

            Console.Write("Finished!");
            Console.Read();
        }

        private static void ParseTheForm(string source)
        {
            var pdfReader = new PdfReader(source);
            var fields = pdfReader.AcroFields;

            var sb = new StringBuilder();
            foreach (var de in pdfReader.AcroFields.Fields.Keys)
            {
                sb.Append(de + Environment.NewLine);
            }
        }

        private static void WriteToFormFields(string source, string destination)
        {
            var bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            var times = new Font(bfTimes, 12, Font.ITALIC, Color.BLACK);

            var pdfReader = new PdfReader(source);
            pdfReader.RemoveUsageRights();
            var pdfStamper = new PdfStamper(pdfReader, new FileStream(destination, FileMode.Create));
            AcroFields fields = pdfStamper.AcroFields;
            
            fields.SetFieldProperty("ShipName","textfont", times.BaseFont,null);
            fields.SetField("ShipName", "Luke Skywalker");

            // close the pdf
            pdfStamper.Close();
        }
    }
}
