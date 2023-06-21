using AutoMapper;
using FrogExhibitionBLL.DTO.FrogDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Web.Http;

namespace FrogExhibitionBLL.Services
{
    public class FrogService : IFrogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FrogService> _logger;
        private readonly IMapper _mapper;
        private readonly ISortHelper<Frog> _sortHelper;
        private readonly IPhotoService _photoService;
        private readonly IFrogPhotoService _frogPhotoService;

        public FrogService(IUnitOfWork unitOfWork, ILogger<FrogService> logger, IMapper mapper, ISortHelper<Frog> sortHelper, IPhotoService photoService, IFrogPhotoService frogPhotoService)
        { 
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _sortHelper = sortHelper;
            _photoService = photoService;
            _frogPhotoService = frogPhotoService;
        }

        public async Task<FrogDetailViewModel> CreateFrog(FrogDtoForCreate frog)
        {
            try
            {
                Frog mappedFrog = _mapper.Map<Frog>(frog);
                Frog createdFrog = await _unitOfWork.Frogs.CreateAsync(mappedFrog);
                // вызов сервисов для создания картинки в wwwroot и сохранения имени картинки в БД
                if(frog.Photos!= null)
                {
                    foreach (var photo in frog.Photos)
                    {
                        var photopath = await _photoService.SavePhotoAsync(photo); // wwwrooot save
                        var frogPhoto = _frogPhotoService.CreateFrogPhotoAsync(new FrogPhoto { PhotoPath = photopath, FrogId = createdFrog.Id });
                    }
                }
                //------
                _logger.LogInformation("Frog Created");
                var res = _mapper.Map<FrogDetailViewModel>(createdFrog);
                res.PhotoPaths = (await _frogPhotoService.GetFrogPhotoPathsAsync(createdFrog.Id)).ToList();
                throw new HttpResponseException()
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, frog);
                throw;
            };
        }
        public async Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogs()
        {
            if (await _unitOfWork.Frogs.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var frogs = await _unitOfWork.Frogs.GetAllAsync(true);
            var mappedfrogs = _mapper.Map<IEnumerable<FrogGeneralViewModel>>(frogs); ///вынести в отдельный метод лучше это
            foreach (var frog in mappedfrogs)
            {
                frog.PhotoPaths = (await _frogPhotoService.GetFrogPhotoPathsAsync(frog.Id)).ToList();
            }
            return mappedfrogs;
        }

        public async Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogs(string sortParams)
        {
            if (await _unitOfWork.Frogs.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var frogs = (await _unitOfWork.Frogs.GetAllAsync(true)).AsQueryable();
            var sortedFrogs = _sortHelper.ApplySort(frogs, sortParams);
            var sortedMappedfrogs = _mapper.Map<IEnumerable<FrogGeneralViewModel>>(sortedFrogs);
            foreach (var frog in sortedMappedfrogs)
            {
                frog.PhotoPaths = (await _frogPhotoService.GetFrogPhotoPathsAsync(frog.Id)).ToList();
            }
            return sortedMappedfrogs;
            
        }


        public async Task<FrogDetailViewModel> GetFrog(Guid id)
        {
            if (await _unitOfWork.Frogs.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var frog = await _unitOfWork.Frogs.GetAsync(id, true);

            if (frog == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var mappedFrog = _mapper.Map<FrogDetailViewModel>(frog);
            mappedFrog.PhotoPaths = (await _frogPhotoService.GetFrogPhotoPathsAsync(mappedFrog.Id)).ToList();
            return mappedFrog;
        }

        //public async Task UpdateFrog(Guid id, FrogDtoForUpdate frog)
        //{
        //    try
        //    {
        //        if (!await _unitOfWork.Frogs.EntityExists(id))
        //        {
        //            throw new NotFoundException("Entity not found");
        //        }
        //        Frog mappedFrog = _mapper.Map<Frog>(frog);
        //        mappedFrog.Id = id;
        //        await _unitOfWork.Frogs.UpdateAsync(mappedFrog);
        //        // вызов сервисов для создания картинки в wwwroot и сохранения имени картинки в БД
        //        foreach (var photo in frog.Photos)
        //        {
        //            var photopath = await _photoService.SavePhotoAsync(photo); // wwwrooot save
        //            await _frogPhotoService.DeleteFrogPhotosAsync(id);
        //            var frogPhoto = _frogPhotoService.CreateFrogPhotoAsync(new FrogPhoto { PhotoPath = photopath, FrogId = id });
        //        }
        //        //------
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!await _unitOfWork.Frogs.EntityExists(id))
        //        {
        //            throw new NotFoundException("Entity not found due to possible concurrency");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        public async Task UpdateFrog(FrogDtoForUpdate frog) // костыли?
        {
            try
            {
                if (!await _unitOfWork.Frogs.EntityExists(frog.Id))
                {
                    throw new NotFoundException("Entity not found");
                }
                Frog mappedFrog = _mapper.Map<Frog>(frog);
                mappedFrog.Id = frog.Id;
                await _unitOfWork.Frogs.UpdateAsync(mappedFrog);
                // вызов сервисов для создания картинки в wwwroot и сохранения имени картинки в БД
                await _frogPhotoService.DeleteFrogPhotosAsync(frog.Id);
                if(frog.Photos != null)
                {
                    foreach (var photo in frog.Photos)
                    {
                        var photopath = await _photoService.SavePhotoAsync(photo); // wwwrooot save
                        var frogPhoto = _frogPhotoService.CreateFrogPhotoAsync(new FrogPhoto { PhotoPath = photopath, FrogId = frog.Id });
                    }
                }
                //------
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.Frogs.EntityExists(frog.Id))
                {
                    throw new NotFoundException("Entity not found due to possible concurrency");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteFrog(Guid id)
        {
            if (await _unitOfWork.Frogs.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var frog = await _unitOfWork.Frogs.GetAsync(id);

            if (frog == null)
            {
                throw new NotFoundException("Entity not found");
            }

            await _unitOfWork.Frogs.DeleteAsync(frog.Id);
        }

    }
}
