using FrogExhibitionDAL.Models;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IFrogPhotoService
    {
        Task<bool> CreateFrogPhotoAsync(FrogPhoto frogPhoto);
        IEnumerable<string> GetFrogPhotoPaths(Guid frogId);
        Task<IEnumerable<string>> GetFrogPhotoPathsAsync(Guid frogId);
        Task<bool> DeleteFrogPhotosAsync(Guid frogId);
    }
}