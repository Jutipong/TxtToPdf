using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConvertToPDF("D:\\TEXT198.TXT", "D:\\TEXT198.PDF");
            //ConvertToPDF("D:\\TEXT198-UTF8.TXT", "D:\\TEXT198-UTF8.PDF");
        }

        public static void ConvertToPDF(string pathInput, string pathOutput)
        {
            using (FileStream fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), pathOutput), FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                var Doc = new Document(PageSize.A4.Rotate());
                var writer = PdfWriter.GetInstance(Doc, fs);
                Doc.Open();
                Doc.NewPage();
                var ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.TTF");
                var baseFont = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var font = new Font(baseFont, 8, Font.NORMAL);
                //using (var str = new StreamReader(pathInput, Encoding.UTF8))
                using (var str = new StreamReader(pathInput, Encoding.GetEncoding("windows-874")))
                {
                    Doc.Add(new Phrase(str.ReadToEnd(), font));
                    Doc.Close();
                } 
            }
        }
    }
}
