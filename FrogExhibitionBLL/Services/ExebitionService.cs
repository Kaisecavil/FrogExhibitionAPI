using AutoMapper;
using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IHelper;
using FrogExhibitionBLL.Interfaces.IService;
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
        private readonly ISortHelper<ExhibitionDetailViewModel> _sortHelper;
        private readonly IFrogPhotoService _frogPhotoService;

        public ExhibitionService(IUnitOfWork unitOfWork, ILogger<ExhibitionService> logger, IMapper mapper, ISortHelper<ExhibitionDetailViewModel> sortHelper, IFrogPhotoService frogPhotoService)
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

        public async Task<IEnumerable<ExhibitionDetailViewModel>> GetAllExhibitionsAsync()
        {
            if (await _unitOfWork.Exhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }

            var result = await _unitOfWork.Exhibitions.GetAllAsync(true);
            return _mapper.Map<IEnumerable<ExhibitionDetailViewModel>>(result);
        }

        public async Task<ExhibitionDetailViewModel> GetExhibitionAsync(Guid id)
        {
            if (await _unitOfWork.Exhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var exebition = await _unitOfWork.Exhibitions.GetAsync(id, true);

            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }

            return _mapper.Map<ExhibitionDetailViewModel>(exebition);
        }

        public async Task UpdateExhibitionAsync(Guid id, ExhibitionDtoForCreate exebition)
        {

            try
            {
                if (!await _unitOfWork.Exhibitions.EntityExists(id))
                {
                    throw new NotFoundException("Entity not found");
                }
                var mappedExhibition = _mapper.Map<Exhibition>(exebition);
                mappedExhibition.Id = id;
                await _unitOfWork.Exhibitions.UpdateAsync(mappedExhibition);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.Exhibitions.EntityExists(id))
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
            var votes = await _unitOfWork.Votes.GetAllAsync();
            var nesVotes = votes.Join(frogsOnExhibition, v => v.FrogOnExhibitionId, f => f.Id, (v, f) => new { FrogOnExhibition = f, Vote = v });
            var group = nesVotes.GroupBy(o => o.FrogOnExhibition.FrogId).Select(o => (new { key = o.Key, Count = o.Count() }));
            var order = group.OrderBy(o => o.Count).Reverse();
            var res = from o in order
                      let obj = _unitOfWork.Frogs.Get(o.key) // ne async????
                      select new FrogRatingViewModel
                      {
                          Id = obj.Id,
                          VotesCount = o.Count,
                          Color = obj.Color,
                          HouseKeepable = obj.HouseKeepable,
                          CurrentAge = obj.CurrentAge,
                          MaxAge = obj.MaxAge,
                          PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(obj.Id).ToList(),
                          Genus = obj.Genus,
                          Habitat = obj.Habitat,
                          Weight = obj.Weight,
                          Sex = obj.Sex,
                          Poisonous = obj.Poisonous,
                          Size = obj.Size,
                          Species = obj.Species
                      };
            return res.ToList();
        }

        public async Task<IEnumerable<ExhibitionDetailViewModel>> GetAllExhibitionsAsync(string sortParams)
        {
            if (await _unitOfWork.Exhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var exebitions = (_mapper.Map<IEnumerable<ExhibitionDetailViewModel>>(await _unitOfWork.Exhibitions.GetAllAsync(true))).AsQueryable();
            var sortedExhibitions = _sortHelper.ApplySort(exebitions, sortParams);
            return sortedExhibitions.ToList();
        }
    }
}
