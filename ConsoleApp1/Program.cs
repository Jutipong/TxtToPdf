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
            ConvertToPDF("D:\\TLNB424F.TXT", "D:\\TLNB424F.PDF");
            //ConvertToPDF("D:\\TEXT198-UTF8.TXT", "D:\\TEXT198-UTF8.PDF");
        }

        public static void ConvertToPDF(string pathInput, string pathOutput)
        {
            using (FileStream fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), pathOutput), FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var Doc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                var writer = PdfWriter.GetInstance(Doc, fs);
                Doc.Open();
                //Doc.NewPage();
                //var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), @"C:\Users\t621506\AppData\Local\Microsoft\Windows\Fonts\THSarabunNew Bold.ttf");
                var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.TTF");
                //var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "THSarabunNew.TTF");
                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var font = new Font(baseFont, 6, Font.NORMAL);
                //using (var str = new StreamReader(pathInput, Encoding.UTF8))

                var dataPageList = new PagesModel();
                var line = "";

                using (var str = new StreamReader(pathInput, Encoding.GetEncoding("windows-874")))
                {
                    var indexPage = -1;
                    while ((line = str.ReadLine()) != null)
                    {
                        if (line.Contains("Date printed"))
                        {
                            dataPageList.Pages.Add(new RowModel());
                            indexPage++;
                        }
                        dataPageList.Pages[indexPage].Rows.Add(line);
                    }
                }

                dataPageList.Pages.ForEach(page =>
                {
                    Doc.NewPage();
                    page.Rows.ForEach(row =>
                    {
                        Doc.Add(new Phrase(row + "\n", font));
                    });
                });

                Doc.Close();
            }
        }
    }

    public class PagesModel
    {
        public List<RowModel> Pages { get; set; } = new List<RowModel>();
    }

    public class RowModel
    {
        public List<string> Rows { get; set; } = new List<string>();
    }
}
