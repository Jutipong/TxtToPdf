using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        private static readonly string rootFileOutput = ConfigurationManager.AppSettings["OutputFilePath"];
        private static readonly string rootFileInput = ConfigurationManager.AppSettings["InputFilePath"];

        static void Main(string[] args)
        {

            CheckDirectory(rootFileInput);
            CheckDirectory(rootFileOutput);

            var directory = new DirectoryInfo(rootFileInput);
            directory.GetFiles("*.TXT").ToList().ForEach(file =>
            {
                ConvertToPDF(file.FullName, $"{file.Name}");
            });
        }

        private static void CheckDirectory(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        public static void ConvertToPDF(string pathInput, string fileName)
        {
            var pathFileOutput = $"{rootFileOutput}\\{fileName}.PDF";
            var newFileName = fileName;

            using (FileStream fs = new FileStream(pathFileOutput, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var Doc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                var writer = PdfWriter.GetInstance(Doc, fs);
                Doc.Open();
                //Doc.NewPage();
                var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.TTF");
                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var font = new Font(baseFont, 6, Font.NORMAL);

                var dataPageList = new PagesModel();
                var line = "";

                using (var str = new StreamReader(pathInput, Encoding.GetEncoding("windows-874")))
                {
                    var indexPage = -1;
                    while ((line = str.ReadLine()) != null)
                    {

                        if (line.Contains("เลขที่บัญชี") && line.Length >= 151)
                        {
                            newFileName = line.Substring(139, 12);
                        }

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

            //Rename File
            File.Move(pathFileOutput, $"{rootFileOutput}\\{newFileName}.PDF");
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
