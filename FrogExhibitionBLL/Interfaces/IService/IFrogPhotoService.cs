using FrogExhibitionDAL.Models;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IFrogPhotoService
    {
        Task CreateFrogPhotoAsync(FrogPhoto frogPhoto);
        IEnumerable<string> GetFrogPhotoPaths(Guid frogId);
        Task DeleteFrogPhotosAsync(Guid frogId);
    }
}