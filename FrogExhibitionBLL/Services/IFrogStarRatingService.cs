using FrogExhibitionBLL.DTO.FrogStarRatingDTOs;
using FrogExhibitionBLL.ViewModels.FrogStarRatingViewModels;

namespace FrogExhibitionBLL.Services
{
    public interface IFrogStarRatingService
    {
        Task<Guid> CreateFrogStarRatingAsync(FrogStarRatingDtoForCreate frogStarRating);
        Task DeleteFrogStarRatingAsync(Guid id);
        Task<IEnumerable<FrogStarRatingGeneralViewModel>> GetAllFrogStarRatingsAsync();
        Task<FrogStarRatingGeneralViewModel> GetFrogStarRatingAsync(Guid id);
        Task UpdateFrogStarRatingAsync(FrogStarRatingDtoForUpdate frogStarRating);
    }
}