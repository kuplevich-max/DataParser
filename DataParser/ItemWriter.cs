using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace DataParser
{
    class ItemWriter
    {
        private string filename;
        public ItemWriter(string filename)
        {
            this.filename = filename;
        }

        public void WriteToTxt(List<Item> items)
        {
            if (items == null)
            {
                throw new Exception("Данные не были загружены");
            }
            using (var sw = new StreamWriter(filename))
            {
                foreach (var item in items)
                {
                    sw.WriteLine(item.ToString());
                }
            }
        }

        public void WriteToWord(List<Item> items)
        {
            if (items == null)
            {
                throw new Exception("Данные не были загружены");
            }
            Word._Application word_app = new Word.Application();
            object missing = Type.Missing;
            Word._Document word_doc = word_app.Documents.Add(
                ref missing, ref missing, ref missing, ref missing);
            Word.Paragraph para = word_doc.Paragraphs.Add(ref missing);
            foreach(var item in items)
            {
                para.Range.Text = item.ToString();
                para.Range.InsertParagraphAfter();
            }

            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(location);

            Object filename = (Object)(path + "\\" + this.filename);
            word_doc.SaveAs(ref filename, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing);
            object save_changes = false;
            word_doc.Close(ref save_changes, ref missing, ref missing);
            word_app.Quit(ref save_changes, ref missing, ref missing);
        }

        public void WriteToExcel(List<Item> items)
        {
            if (items == null)
            {
                throw new Exception("Данные не были загружены");
            }
            Excel.Application excel_app = new Excel.Application();
            Excel.Workbook workbook = excel_app.Workbooks.Add(Missing.Value);
            Excel.Worksheet sheet = workbook.ActiveSheet;
            sheet.Cells[1, 1] = "Title";
            sheet.Cells[1, 2] = "Link";
            sheet.Cells[1, 3] = "Description";
            sheet.Cells[1, 4] = "PubDate";
            sheet.get_Range("A1", "D1").Font.Bold = true;
            sheet.get_Range("A1", "D1").VerticalAlignment =
            Excel.XlVAlign.xlVAlignCenter;
            int i = 2;
            foreach(var item in items)
            {
                sheet.get_Range($"A{i}:D{i}").Value2 = item.ToString().Split('\n');
                i++;
            }
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var path = Path.GetDirectoryName(location);

            Object filename = (Object)(path + "\\" + this.filename);
            workbook.SaveAs(filename);

            workbook.Close(true, Type.Missing, Type.Missing);
            excel_app.Quit();

        }
    }
}
