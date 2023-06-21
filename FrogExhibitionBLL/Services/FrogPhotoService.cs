using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.Extensions.Logging;

namespace FrogExhibitionBLL.Services
{
    public class FrogPhotoService : IFrogPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FrogPhotoService> _logger;

        public FrogPhotoService(IUnitOfWork unitOfWork, ILogger<FrogPhotoService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetFrogPhotoPathsAsync(Guid frogId)
        {
            var photoPaths = (await _unitOfWork.FrogPhotos.GetAllAsync()).AsQueryable().Where(p => p.FrogId == frogId).Select(p => p.PhotoPath);
            return photoPaths.ToList();
        }

        public async Task<FrogPhoto> CreateFrogPhotoAsync(FrogPhoto frogPhoto)
        {
            try
            {
                return await _unitOfWork.FrogPhotos.CreateAsync(frogPhoto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            };
        }

        public IEnumerable<string> GetFrogPhotoPaths(Guid frogId)
        {
            var photoPaths = _unitOfWork.FrogPhotos.GetAll().AsQueryable().Where(p => p.FrogId == frogId).Select(p => p.PhotoPath);
            return photoPaths.ToList();
        }

        public async Task DeleteFrogPhotosAsync(Guid frogId)
        {
            try
            {
                var frogPhotos = (await _unitOfWork.FrogPhotos.GetAllAsync()).Where(p => p.FrogId == frogId);
                foreach (var item in frogPhotos)
                {
                    await _unitOfWork.FrogPhotos.DeleteAsync(item.Id);
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }
    }
}
