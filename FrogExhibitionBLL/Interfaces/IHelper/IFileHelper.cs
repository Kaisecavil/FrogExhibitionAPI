using Microsoft.AspNetCore.Http;

namespace FrogExhibitionBLL.Interfaces.IHelper
{
    public interface IFileHelper
    {
        string GetUniqueFileName(string fileName);
        Task<string> SavePhotoAsync(IFormFile file);
    }
}