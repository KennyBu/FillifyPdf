using System;
using System.IO;
using iTextSharp.text;
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

            ParseTheForm(pdfTemplate);
            WriteToFormFields(pdfTemplate, newFile);

            Console.WriteLine("File Created Successfully! Filname:{0}", newFile);
            Console.Write("Finished!");
            Console.Read();
        }

        private static void ParseTheForm(string source)
        {
            var pdfReader = new PdfReader(source);
            
            foreach (var key in pdfReader.AcroFields.Fields.Keys)
            {
                Console.WriteLine(key);
            }
        }

        private static void WriteToFormFields(string source, string destination)
        {
            var theFont = GetTahomaFont();
            var font = new Font(theFont.BaseFont, 12, Font.ITALIC, Color.BLACK);

            var pdfReader = new PdfReader(source);
            pdfReader.RemoveUsageRights();
            var pdfStamper = new PdfStamper(pdfReader, new FileStream(destination, FileMode.Create));
            var fields = pdfStamper.AcroFields;

            fields.SetFieldProperty("ShipName", "textfont", font.BaseFont, null);
            fields.SetField("ShipName", "Luke Skywalker");

            // close the pdf
            pdfStamper.Close();
        }

        public static Font GetTahomaFont()
        {
            const string fontName = "Tahoma";
            if (!FontFactory.IsRegistered(fontName))
            {
                var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf";
                FontFactory.Register(fontPath);
            }
            return FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }
    }
}
