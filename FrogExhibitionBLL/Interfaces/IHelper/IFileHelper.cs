using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrogExhibitionBLL.Interfaces.IHelper
{
    public interface IFileHelper
    {
        string GetExhibitionReportFilePath(string exhibitionName);
        string GetUserReportFilePath(string userId);
        string GetUniqueFileName(string fileName);
        Task<string> SavePhotoAsync(IFormFile file);
        Task<string> SavePhotoAsync(IFormFile file, string directoryPath);
        Task<FileContentResult> GetFileContentResultAsync(string filePath, string contentType, bool deleteFileAfter = false);
    }
}