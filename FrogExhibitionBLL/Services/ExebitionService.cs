using AutoMapper;
using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IHelper;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.CommentViewModels;
using FrogExhibitionBLL.ViewModels.ExhibitionViewModels;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrogExhibitionBLL.Services
{
    public class ExhibitionService : IExhibitionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExhibitionService> _logger;
        private readonly IMapper _mapper;
        private readonly ISortHelper<Exhibition> _sortHelper;
        private readonly IFrogPhotoService _frogPhotoService;

        public ExhibitionService(IUnitOfWork unitOfWork,
            ILogger<ExhibitionService> logger,
            IMapper mapper,
            ISortHelper<Exhibition> sortHelper,
            IFrogPhotoService frogPhotoService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _sortHelper = sortHelper;
            _frogPhotoService = frogPhotoService;
        }

        public async Task<Guid> CreateExhibitionAsync(ExhibitionDtoForCreate exebition)
        {
            try
            {
                var mappedExhibition = _mapper.Map<Exhibition>(exebition);
                await _unitOfWork.Exhibitions.CreateAsync(mappedExhibition);
                _logger.LogInformation("Exhibition Created");
                await _unitOfWork.SaveAsync();
                return mappedExhibition.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, exebition);
                throw;
            };

        }

        public async Task<IEnumerable<ExhibitionGeneralViewModel>> GetAllExhibitionsAsync()
        {
            var result = await _unitOfWork.Exhibitions.GetAllAsync(true);
            return _mapper.Map<IEnumerable<ExhibitionGeneralViewModel>>(result);
        }

        public async Task<ExhibitionDetailViewModel> GetExhibitionAsync(Guid id)
        {
            var exebition = await _unitOfWork.Exhibitions.GetAsync(id);
            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var mappedExhibition = _mapper.Map<ExhibitionDetailViewModel>(exebition);
            mappedExhibition.Frogs = _mapper.Map<List<FrogGeneralViewModel>>(exebition.Frogs);
            mappedExhibition.Frogs
                .ForEach(f => f.Comments = _mapper.Map<List<CommentGeneralViewModel>>(exebition.FrogsOnExhibitions.First(foe => foe.FrogId == f.Id).Comments)); 
            return mappedExhibition;
        }

        public async Task UpdateExhibitionAsync(ExhibitionDtoForUpdate exebition)
        {
            try
            {
                if (await _unitOfWork.Exhibitions.EntityExistsAsync(exebition.Id))
                {
                    var mappedExhibition = _mapper.Map<Exhibition>(exebition);
                    await _unitOfWork.Exhibitions.UpdateAsync(mappedExhibition);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    throw new NotFoundException("Entity not found");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.Exhibitions.EntityExistsAsync(exebition.Id))
                {
                    throw new NotFoundException("Entity not found due to possible concurrency");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteExhibitionAsync(Guid id)
        {
            var exebition = await _unitOfWork.Exhibitions.GetAsync(id);
            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            await _unitOfWork.Exhibitions.DeleteAsync(exebition.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<FrogRatingViewModel>> GetRatingAsync(Guid id)
        {
            var exebition = await _unitOfWork.Exhibitions.GetAsync(id);
            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var frogsOnExhibition = exebition.FrogsOnExhibitions;
            var res = frogsOnExhibition.Select(foe => new FrogRatingViewModel
            {
                Id = foe.Frog.Id,
                VotesCount = foe.Votes.Count,
                VotesSum = foe.Votes.Sum(v => (int)v.ApplicationUser.KnowledgeLevel),
                Color = foe.Frog.Color,
                HouseKeepable = foe.Frog.HouseKeepable,
                CurrentAge = foe.Frog.CurrentAge,
                MaxAge = foe.Frog.MaxAge,
                PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(foe.Frog.Id).ToList(),
                Genus = foe.Frog.Genus,
                Habitat = foe.Frog.Habitat,
                Weight = foe.Frog.Weight,
                Sex = foe.Frog.Sex.ToString(),
                Poisonous = foe.Frog.Poisonous,
                Size = foe.Frog.Size,
                Species = foe.Frog.Species
            });
            return res.ToList();
        }

        public async Task<IEnumerable<ExhibitionGeneralViewModel>> GetAllExhibitionsAsync(string sortParams)
        {
            var exebitions = _unitOfWork.Exhibitions.GetAllQueryable(true);
            var sortedExhibitions = await _sortHelper.ApplySort(exebitions, sortParams).ToListAsync();
            return _mapper.Map<IEnumerable<ExhibitionGeneralViewModel>>(sortedExhibitions);
        }
    }
}
