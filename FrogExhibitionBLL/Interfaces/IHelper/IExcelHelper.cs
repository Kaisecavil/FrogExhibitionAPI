namespace FrogExhibitionBLL.Interfaces.IHelper
{
    public interface IExcelHelper
    {
        void AppendObjectsToSpreadsheet(List<object> objects, string filePath, string tableHeader, bool printColumnNamesAgain = true);
        void CreateSpreadsheetFromObjects(List<object> objects, string filePath, string tableHeader);
    }
}