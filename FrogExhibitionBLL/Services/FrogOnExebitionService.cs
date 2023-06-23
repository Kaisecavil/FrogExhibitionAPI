using AutoMapper;
using FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.FrogOnExhibitionViewModels;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<Guid> CreateFrogOnExhibitionAsync(FrogOnExhibitionDtoForCreate frogOnExhibition)
        {
            try
            {
                var mappedFrogOnExhibition = _mapper.Map<FrogOnExhibition>(frogOnExhibition);
                await _unitOfWork.FrogOnExhibitions.CreateAsync(mappedFrogOnExhibition);
                _logger.LogInformation("FrogOnExhibition Created");
                await _unitOfWork.SaveAsync();
                return mappedFrogOnExhibition.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, frogOnExhibition);
                throw;
            };
        }

        public async Task<IEnumerable<FrogOnExhibitionDetailViewModel>> GetAllFrogOnExhibitionsAsync()
        {
            var result = await _unitOfWork.FrogOnExhibitions.GetAllAsync(true);
            return _mapper.Map<IEnumerable<FrogOnExhibitionDetailViewModel>>(result);
        }

        public async Task<FrogOnExhibitionDetailViewModel> GetFrogOnExhibitionAsync(Guid id)
        {
            var frogOnExhibition = await _unitOfWork.FrogOnExhibitions.GetAsync(id, true);
            if (frogOnExhibition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            return _mapper.Map<FrogOnExhibitionDetailViewModel>(frogOnExhibition);
        }

        public async Task UpdateFrogOnExhibitionAsync(FrogOnExhibitionDtoForUpdate frogOnExhibition)
        {
            try
            {
                if (await _unitOfWork.FrogOnExhibitions.EntityExistsAsync(frogOnExhibition.Id))
                {
                    var mappedFrogOnExhibition = _mapper.Map<FrogOnExhibition>(frogOnExhibition);
                    await _unitOfWork.FrogOnExhibitions.UpdateAsync(mappedFrogOnExhibition);
                    await _unitOfWork.SaveAsync();
                }
                else 
                {
                    throw new NotFoundException("Entity not found");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.FrogOnExhibitions.EntityExistsAsync(frogOnExhibition.Id))
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

        public async Task DeleteFrogOnExhibitionAsync(Guid id)
        {
            var frogOnExhibition = await _unitOfWork.FrogOnExhibitions.GetAsync(id);
            if (frogOnExhibition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            await _unitOfWork.FrogOnExhibitions.DeleteAsync(frogOnExhibition.Id);
            await _unitOfWork.SaveAsync();
        }


    }
}
