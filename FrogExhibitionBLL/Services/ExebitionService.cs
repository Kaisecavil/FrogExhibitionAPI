using AutoMapper;
using FrogExhibitionBLL.Interfaces.IService;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionBLL.Services
{
    public class ExhibitionService : IExhibitionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExhibitionService> _logger;
        private readonly IMapper _mapper;
        private readonly ISortHelper<ExhibitionDtoDetail> _sortHelper;
        private readonly IFrogPhotoService _frogPhotoService;

        public ExhibitionService(IUnitOfWork unitOfWork, ILogger<ExhibitionService> logger, IMapper mapper, ISortHelper<ExhibitionDtoDetail> sortHelper, IFrogPhotoService frogPhotoService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _sortHelper = sortHelper;
            _frogPhotoService = frogPhotoService;
        }

        public async Task<ExhibitionDtoDetail> CreateExhibition(ExhibitionDtoForCreate exebition)
        {

            try
            {
                var mappedExhibition = _mapper.Map<Exhibition>(exebition);
                var createdExhibition = await _unitOfWork.Exhibitions.CreateAsync(mappedExhibition);
                _logger.LogInformation("Exhibition Created");
                return _mapper.Map<ExhibitionDtoDetail>(createdExhibition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, exebition);
                throw;
            };

        }

        public async Task<IEnumerable<ExhibitionDtoDetail>> GetAllExhibitions()
        {
            if (await _unitOfWork.Exhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }

            var result = await _unitOfWork.Exhibitions.GetAllAsync(true);
            return _mapper.Map<IEnumerable<ExhibitionDtoDetail>>(result);
        }

        public async Task<ExhibitionDtoDetail> GetExhibition(Guid id)
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

            return _mapper.Map<ExhibitionDtoDetail>(exebition);
        }

        public async Task UpdateExhibition(Guid id, ExhibitionDtoForCreate exebition)
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

        public async Task DeleteExhibition(Guid id)
        {
            if (await _unitOfWork.Exhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var exebition = await _unitOfWork.Exhibitions.GetAsync(id);

            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }

            await _unitOfWork.Exhibitions.DeleteAsync(exebition.Id);
        }

        public async Task<IEnumerable<FrogDtoRating>> GetRating(Guid id)
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
                      select new FrogDtoRating
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

        public async Task<IEnumerable<ExhibitionDtoDetail>> GetAllExhibitions(string sortParams)
        {
            if (await _unitOfWork.Exhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var exebitions = (_mapper.Map<IEnumerable<ExhibitionDtoDetail>>(await _unitOfWork.Exhibitions.GetAllAsync(true))).AsQueryable();
            var sortedExhibitions = _sortHelper.ApplySort(exebitions, sortParams);
            return sortedExhibitions.ToList();
        }
    }
}
