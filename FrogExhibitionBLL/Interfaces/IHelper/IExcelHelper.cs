namespace FrogExhibitionBLL.Interfaces.IHelper
{
    public interface IExcelHelper
    {
        void AppendObjectsToSpreadsheet<T>(List<T> objects, string filePath, string tableHeader, bool printColumnNamesAgain = true);
        void CreateSpreadsheetFromObjects<T>(List<T> objects, string filePath, string tableHeader);
    }
}