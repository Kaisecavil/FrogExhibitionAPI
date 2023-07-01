using Microsoft.AspNetCore.Http;

namespace FrogExhibitionBLL.Interfaces.IHelper
{
    public interface IFileHelper
    {
        string GetExhibitionReportFilePath(string exhibitionName);
        string GetUniqueFileName(string fileName);
        Task<string> SavePhotoAsync(IFormFile file);
    }
}