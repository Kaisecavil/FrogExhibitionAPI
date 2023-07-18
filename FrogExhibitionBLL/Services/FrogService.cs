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
                if(frog.Photos!= null)
                {
                    foreach (var photo in frog.Photos)
                    {
                        var photopath = await _fileHelper.SavePhotoAsync(photo);
                        var frogPhoto = _frogPhotoService.CreateFrogPhotoAsync(new FrogPhoto { PhotoPath = photopath, FrogId = mappedFrog.Id });
                    }
                }
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
            foreach (var frog in mappedfrogs)
            {
                frog.PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(frog.Id).ToList();
            }
            return mappedfrogs;
        }

        public async Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogsAsync(string sortParams)
        {
            var frogs = (await _unitOfWork.Frogs.GetAllAsync(true)).AsQueryable();
            var sortedFrogs = _sortHelper.ApplySort(frogs, sortParams);
            var sortedMappedfrogs = _mapper.Map<IEnumerable<FrogGeneralViewModel>>(sortedFrogs);
            foreach (var frog in sortedMappedfrogs)
            {
                frog.PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(frog.Id).ToList();
            }
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
            //var commentsList = new List<Comment>();
            //frog.FrogsOnExhibitions.ForEach(foe => commentsList.AddRange(foe.Comments));
            //mappedFrog.Comments = _mapper.Map<List<CommentGeneralViewModel>>(commentsList.OrderBy(c => c.CreationDate));
            //mappedFrog.PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(mappedFrog.Id).ToList();
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
                if(frog.Photos != null)
                {
                    foreach (var photo in frog.Photos)
                    {
                        var photopath = await _fileHelper.SavePhotoAsync(photo);
                        var frogPhoto = _frogPhotoService.CreateFrogPhotoAsync(new FrogPhoto { PhotoPath = photopath, FrogId = frog.Id });
                    }
                }
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
