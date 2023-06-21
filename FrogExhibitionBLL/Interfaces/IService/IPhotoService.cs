using Microsoft.AspNetCore.Http;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IPhotoService
    {
        Task<string> SavePhotoAsync(IFormFile file);
    }
}