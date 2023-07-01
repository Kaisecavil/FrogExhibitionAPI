using AutoMapper;
using FrogExhibitionBLL.DTO.FrogStarRatingDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IProvider;
using FrogExhibitionBLL.ViewModels.FrogStarRatingViewModels;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrogExhibitionBLL.Services
{
    public class FrogStarRatingService : IFrogStarRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FrogStarRatingService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;

        public FrogStarRatingService(IUnitOfWork unitOfWork,
            ILogger<FrogStarRatingService> logger,
            IMapper mapper,
            IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userProvider = userProvider;
        }

        public async Task<Guid> CreateFrogStarRatingAsync(FrogStarRatingDtoForCreate frogStarRating)
        {
            try
            {
                var currentUserId = await _userProvider.GetUserIdAsync();
                if(_unitOfWork.FrogStarRatings.GetAllQueryable(true)
                    .Any(fsr => fsr.FrogId == frogStarRating.FrogId &&
                         fsr.ApplicationUserId == currentUserId))
                {
                    throw new DbUpdateException("User can leave only one star rating per one frog");
                }
                var mappedFrogStarRating = _mapper.Map<FrogStarRating>(frogStarRating);
                mappedFrogStarRating.ApplicationUserId = currentUserId;
                await _unitOfWork.FrogStarRatings.UpdateAsync(mappedFrogStarRating);
                await _unitOfWork.SaveAsync();
                return mappedFrogStarRating.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, frogStarRating);
                throw;
            };
        }

        public async Task<IEnumerable<FrogStarRatingGeneralViewModel>> GetAllFrogStarRatingsAsync()
        {
            var result = await _unitOfWork.FrogStarRatings.GetAllAsync(true);
            return _mapper.Map<IEnumerable<FrogStarRatingGeneralViewModel>>(result);
        }

        public async Task<FrogStarRatingGeneralViewModel> GetFrogStarRatingAsync(Guid id)
        {
            var frogStarRating = await _unitOfWork.FrogStarRatings.GetAsync(id, true);
            if (frogStarRating == null)
            {
                throw new NotFoundException("Entity not found");
            }
            return _mapper.Map<FrogStarRatingGeneralViewModel>(frogStarRating);
        }

        public async Task UpdateFrogStarRatingAsync(FrogStarRatingDtoForUpdate frogStarRating)
        {
            try
            {

                if (await _unitOfWork.FrogStarRatings.EntityExistsAsync(frogStarRating.Id))
                {
                    var currentUserId = await _userProvider.GetUserIdAsync();
                    var frogStarRatingToUpdate = await _unitOfWork.FrogStarRatings.GetAsync(frogStarRating.Id);
                    if (frogStarRatingToUpdate.ApplicationUserId == currentUserId)
                    {
                        var mappedFrogStarRating = _mapper.Map<FrogStarRating>(frogStarRating);
                        mappedFrogStarRating.ApplicationUserId = currentUserId;
                        await _unitOfWork.FrogStarRatings.UpdateAsync(mappedFrogStarRating);
                        await _unitOfWork.SaveAsync();
                    }
                    else
                    {
                        throw new BadRequestException("You can't update frogStarRatings of other users");
                    }

                }
                else
                {
                    throw new NotFoundException("Entity not found");
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _unitOfWork.FrogStarRatings.EntityExistsAsync(frogStarRating.Id))
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
                _logger.LogError(ex.Message, frogStarRating);
                throw;
            };
        }

        public async Task DeleteFrogStarRatingAsync(Guid id)
        {
            var frogStarRating = await _unitOfWork.FrogStarRatings.GetAsync(id);
            if (frogStarRating == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var currentUserId = await _userProvider.GetUserIdAsync();
            if (frogStarRating.ApplicationUserId == currentUserId)
            {
                await _unitOfWork.FrogStarRatings.DeleteAsync(frogStarRating.Id);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new BadRequestException("You can't delete other users frogStarRatings");
            }
        }
    }
}
