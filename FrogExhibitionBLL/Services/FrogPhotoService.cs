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

        public IEnumerable<string> GetFrogPhotoPaths(Guid frogId)
        {
            var photoPaths = _unitOfWork.FrogPhotos.GetAll().AsQueryable().Where(p => p.FrogId == frogId).Select(p => p.PhotoPath);
            return photoPaths.ToList();
        }

        public async Task<bool> CreateFrogPhotoAsync(FrogPhoto frogPhoto)
        {
            try
            {
                await _unitOfWork.FrogPhotos.CreateAsync(frogPhoto);
                return await _unitOfWork.SaveAsync() == 1;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            };
        }

        public async Task<bool> DeleteFrogPhotosAsync(Guid frogId)
        {
            try
            {
                var frogPhotos = (await _unitOfWork.FrogPhotos.GetAllAsync()).Where(p => p.FrogId == frogId);
                _unitOfWork.FrogPhotos.DeleteRange(frogPhotos);
                return await _unitOfWork.SaveAsync() == frogPhotos.Count();
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }
    }
}
