using Microsoft.AspNetCore.Http;

namespace FrogExhibitionBLL.Interfaces.IHelper
{
    public interface IFileHelper
    {
        string GetExhibitionReportFilePath(string exhibitionName);
        string GetUserReportFilePath(string userId);
        string GetUniqueFileName(string fileName);
        Task<string> SavePhotoAsync(IFormFile file);
    }
}