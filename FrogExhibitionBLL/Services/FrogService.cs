using AutoMapper;
using FrogExhibitionBLL.DTO.FrogDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IHelper;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrogExhibitionBLL.Services
{
    public class FrogService : IFrogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FrogService> _logger;
        private readonly IMapper _mapper;
        private readonly ISortHelper<Frog> _sortHelper;
        private readonly IFrogPhotoService _frogPhotoService;
        private readonly IFileHelper _fileHelper;

        public FrogService(IUnitOfWork unitOfWork,
            ILogger<FrogService> logger,
            IMapper mapper,
            ISortHelper<Frog> sortHelper,
            IFrogPhotoService frogPhotoService,
            IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _sortHelper = sortHelper;
            _frogPhotoService = frogPhotoService;
            _fileHelper = fileHelper;
        }

        public async Task<Guid> CreateFrogAsync(FrogDtoForCreate frog)
        {
            try
            {
                Frog mappedFrog = _mapper.Map<Frog>(frog);
                await _unitOfWork.Frogs.CreateAsync(mappedFrog);
                frog.Photos?.Select(p => _fileHelper.SavePhotoAsync(p)).ToList()
                    .ForEach(async photopath => await _frogPhotoService
                        .CreateFrogPhotoAsync(
                            new FrogPhoto 
                            { 
                                PhotoPath = photopath.Result,
                                FrogId = mappedFrog.Id
                            }));
                await _unitOfWork.SaveAsync();
                return mappedFrog.Id;
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError(ex.Message, frog);
                throw new BadRequestException($"Value \"{frog.Sex}\" is not valid for the Sex field");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, frog);
                throw;
            };
        }
        public async Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogsAsync()
        {
            var frogs = await _unitOfWork.Frogs.GetAllAsync(true);
            var mappedfrogs = _mapper.Map<IEnumerable<FrogGeneralViewModel>>(frogs);
            mappedfrogs.ToList()
                .ForEach(f => f.PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(f.Id).ToList());
            return mappedfrogs;
        }

        public async Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogsAsync(string sortParams)
        {
            var frogs = (await _unitOfWork.Frogs.GetAllAsync(true)).AsQueryable();
            var sortedFrogs = _sortHelper.ApplySort(frogs, sortParams);
            var sortedMappedfrogs = _mapper.Map<IEnumerable<FrogGeneralViewModel>>(sortedFrogs);
            sortedMappedfrogs.ToList()
                .ForEach(f => f.PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(f.Id).ToList());
            return sortedMappedfrogs;

        }

        public async Task<FrogDetailViewModel> GetFrogAsync(Guid id)
        {
            var frog = await _unitOfWork.Frogs.GetAsync(id);
            if (frog == null)
            {
                throw new NotFoundException("Entity not found");
            }

            var mappedFrog = _mapper.Map<FrogDetailViewModel>(frog);
            return mappedFrog;
        }

        public async Task UpdateFrogAsync(FrogDtoForUpdate frog)
        {
            try
            {
                if (!await _unitOfWork.Frogs.EntityExistsAsync(frog.Id))
                {
                    throw new NotFoundException("Entity not found");
                }

                Frog mappedFrog = _mapper.Map<Frog>(frog);
                await _unitOfWork.Frogs.UpdateAsync(mappedFrog);
                await _frogPhotoService.DeleteFrogPhotosAsync(frog.Id);
                frog.Photos?.Select(p => _fileHelper.SavePhotoAsync(p)).ToList()
                    .ForEach(async photopath => await _frogPhotoService
                        .CreateFrogPhotoAsync(
                            new FrogPhoto
                            {
                                PhotoPath = photopath.Result,
                                FrogId = mappedFrog.Id
                            }));
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.Frogs.EntityExistsAsync(frog.Id))
                {
                    throw new NotFoundException("Entity not found due to possible concurrency");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteFrogAsync(Guid id)
        {
            var frog = await _unitOfWork.Frogs.GetAsync(id);

            if (frog == null)
            {
                throw new NotFoundException("Entity not found");
            }

            await _unitOfWork.Frogs.DeleteAsync(frog.Id);
            await _unitOfWork.SaveAsync();
        }

    }
}
