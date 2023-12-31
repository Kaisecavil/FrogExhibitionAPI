﻿using FrogExhibitionBLL.Interfaces.IHelper;
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
        private readonly IFileHelper _fileHelper;

        public FrogPhotoService(IUnitOfWork unitOfWork,
            ILogger<FrogPhotoService> logger,
            IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _fileHelper = fileHelper;
        }

        public IEnumerable<string> GetFrogPhotoPaths(Guid frogId)
        {
            var photoPaths = _unitOfWork.FrogPhotos.GetAllQueryable(true)
                .Where(p => p.FrogId == frogId)
                .Select(p => p.PhotoPath);
            return photoPaths.ToList();
        }

        public async Task CreateFrogPhotoAsync(FrogPhoto frogPhoto)
        {
            try
            {
                await _unitOfWork.FrogPhotos.CreateAsync(frogPhoto);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            };
        }

        public async Task DeleteFrogPhotosAsync(Guid frogId)
        {
            try
            {
                var frogPhotos = (await _unitOfWork.FrogPhotos.GetAllAsync())
                    .Where(p => p.FrogId == frogId);
                _unitOfWork.FrogPhotos.DeleteRange(frogPhotos);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }
    }
}
