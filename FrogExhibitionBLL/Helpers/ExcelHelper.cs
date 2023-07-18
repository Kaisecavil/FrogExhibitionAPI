using FrogExhibitionBLL.Interfaces.IHelper;
using Microsoft.Office.Interop.Excel;

namespace FrogExhibitionBLL.Helpers
{
    public class ExcelHelper : IExcelHelper
    {
        public void CreateSpreadsheetFromObjects<T>(List<T> objects, string filePath, string tableHeader)
        {
            var excelApp = new Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = workbook.Worksheets[1] as Worksheet;
            WriteDataHeaders<T>(worksheet, 1, tableHeader);
            WriteDataRows(worksheet, objects, 3, 1); 
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();

            ReleaseCOMObjects(worksheet);
            ReleaseCOMObjects(workbook);
            ReleaseCOMObjects(excelApp);
        }
       
        public void AppendObjectsToSpreadsheet<T>(List<T> objects, string filePath, string tableHeader, bool printColumnNamesAgain = true)
        {
            var excelApp = new Application();
            var workbook = excelApp.Workbooks.Open(filePath);
            var worksheet = workbook.Worksheets[1] as Worksheet;

            // Determine the next available row in the worksheet
            int lastRow = worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row + 4;
            
            // Write headers if the worksheet is empty
            if (printColumnNamesAgain)
            {
                WriteDataHeaders<T>(worksheet, lastRow - 1, tableHeader);
            }

            WriteDataRows(worksheet, objects, lastRow, 1);

            workbook.Save();
            workbook.Close();
            excelApp.Quit();

            ReleaseCOMObjects(worksheet);
            ReleaseCOMObjects(workbook);
            ReleaseCOMObjects(excelApp);
        }

        private void WriteDataHeaders<T>(Worksheet worksheet, int rowAdjustment, string tableHeader)
        {
            var properties = typeof(T).GetProperties();
            if (properties.Length != 0)
            {
                worksheet.Cells[1 + rowAdjustment, 1] = tableHeader;
                for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    var property = properties[columnIndex];
                    var header = ReplaceCapitalLetters(property.Name);
                    worksheet.Cells[1 + rowAdjustment,columnIndex + 1] = header;

                    var columnRange = worksheet.Columns[columnIndex + 1];
                    columnRange.AutoFit();
                }
            }

        }

        private void WriteDataRows<T>(Worksheet worksheet, List<T> objects, int rowAdjustment, int colAdjustment)
        {
            var properties = typeof(T).GetProperties();
            for (int rowIndex = 0; rowIndex < objects.Count; rowIndex++)
            {
                var currentObject = objects[rowIndex];
                for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    var property = properties[columnIndex];
                    var value = property.GetValue(currentObject);
                    worksheet.Cells[rowIndex + rowAdjustment, columnIndex + colAdjustment] = value?.ToString();
                }
            }

        }

        private string ReplaceCapitalLetters(string input)
        {
            string output = "";
            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    output += " " + char.ToLower(c);
                }
                else
                {
                    output += c;
                }
            }

            return output.Trim();
        }

        private void ReleaseCOMObjects(object obj)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            obj = null;
            GC.Collect();
        }
    }
}
