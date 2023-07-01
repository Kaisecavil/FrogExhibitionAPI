namespace FrogExhibitionBLL.Interfaces.IHelper
{
    public interface IExcelHelper
    {
        void CreateSpreadsheetFromObjects(List<object> objects, string filePath);
    }
}