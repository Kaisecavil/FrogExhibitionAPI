using AutoMapper;
using FrogExhibitionBLL.Interfaces.IService;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionBLL.Services
{
    public class FrogOnExhibitionService : IFrogOnExhibitionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FrogOnExhibitionService> _logger;
        private readonly IMapper _mapper;

        public FrogOnExhibitionService(IUnitOfWork unitOfWork, ILogger<FrogOnExhibitionService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<FrogOnExhibitionDtoDetail> CreateFrogOnExhibition(FrogOnExhibitionDtoForCreate frogOnExhibition)
        {
            try
            {
                var mappedFrogOnExhibition = _mapper.Map<FrogOnExhibition>(frogOnExhibition);
                var createdFrogOnExhibition = await _unitOfWork.FrogOnExhibitions.CreateAsync(mappedFrogOnExhibition);
                _logger.LogInformation("FrogOnExhibition Created");
                return _mapper.Map<FrogOnExhibitionDtoDetail>(createdFrogOnExhibition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, frogOnExhibition);
                throw;
            };
        }

        public async Task<IEnumerable<FrogOnExhibitionDtoDetail>> GetAllFrogOnExhibitions()
        {
            if (await _unitOfWork.FrogOnExhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var result = await _unitOfWork.FrogOnExhibitions.GetAllAsync(true);
            return _mapper.Map<IEnumerable<FrogOnExhibitionDtoDetail>>(result);
        }

        public async Task<FrogOnExhibitionDtoDetail> GetFrogOnExhibition(Guid id)
        {
            if (await _unitOfWork.FrogOnExhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var frogOnExhibition = await _unitOfWork.FrogOnExhibitions.GetAsync(id, true);

            if (frogOnExhibition == null)
            {
                throw new NotFoundException("Entity not found");
            }

            return _mapper.Map<FrogOnExhibitionDtoDetail>(frogOnExhibition);
        }

        public async Task UpdateFrogOnExhibition(Guid id, FrogOnExhibitionDtoForCreate frogOnExhibition)
        {

            try
            {
                if (!await _unitOfWork.FrogOnExhibitions.EntityExists(id))
                {
                    throw new NotFoundException("Entity not found");
                }
                var mappedFrogOnExhibition = _mapper.Map<FrogOnExhibition>(frogOnExhibition);
                mappedFrogOnExhibition.Id = id;
                await _unitOfWork.FrogOnExhibitions.UpdateAsync(mappedFrogOnExhibition);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.FrogOnExhibitions.EntityExists(id))
                {
                    throw new NotFoundException("Entity not found due to possible concurrency");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, frogOnExhibition);
                throw;
            }
        }

        public async Task DeleteFrogOnExhibition(Guid id)
        {
            if (await _unitOfWork.FrogOnExhibitions.IsEmpty())
            {
                throw new NotFoundException("Entity not found due to emptines of db");
            }
            var frogOnExhibition = await _unitOfWork.FrogOnExhibitions.GetAsync(id);

            if (frogOnExhibition == null)
            {
                throw new NotFoundException("Entity not found");
            }

            await _unitOfWork.FrogOnExhibitions.DeleteAsync(frogOnExhibition.Id);
        }


    }
}
