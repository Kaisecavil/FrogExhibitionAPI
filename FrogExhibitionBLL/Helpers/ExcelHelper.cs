using FrogExhibitionBLL.Interfaces.IHelper;
using Microsoft.Office.Interop.Excel;

namespace FrogExhibitionBLL.Helpers
{
    public class ExcelHelper : IExcelHelper
    {
        public void CreateSpreadsheetFromObjects(List<object> objects, string filePath)
        {
            var excelApp = new Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = workbook.Worksheets[1] as Worksheet;

            // Write headers using property names of the first object
            var properties = objects.Count > 0
                ? objects[0].GetType().GetProperties()
                : null;
            if (properties != null)
            {
                for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    var property = properties[columnIndex];
                    var header = property.Name;
                    worksheet.Cells[1, columnIndex + 1] = header;
                }
            }

            // Write data rows
            for (int rowIndex = 0; rowIndex < objects.Count; rowIndex++)
            {
                var currentObject = objects[rowIndex];
                properties = currentObject.GetType().GetProperties();

                for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    var property = properties[columnIndex];
                    var value = property.GetValue(currentObject);
                    worksheet.Cells[rowIndex + 2, columnIndex + 1] = value?.ToString();
                }
            }

            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();

            ReleaseCOMObjects(worksheet);
            ReleaseCOMObjects(workbook);
            ReleaseCOMObjects(excelApp);
        }

        private void ReleaseCOMObjects(object obj)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            obj = null;
            GC.Collect();
        }
    }
}
