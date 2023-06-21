using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.ViewModels.ExhibitionViewModels;
using FrogExhibitionBLL.ViewModels.FrogViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IExhibitionService
    {
        Task<ExhibitionDetailViewModel> CreateExhibition(ExhibitionDtoForCreate exebition);
        Task DeleteExhibition(Guid id);
        Task<IEnumerable<ExhibitionDetailViewModel>> GetAllExhibitions();
        Task<ExhibitionDetailViewModel> GetExhibition(Guid id);
        Task UpdateExhibition(Guid id, ExhibitionDtoForCreate exebition);
        Task<IEnumerable<FrogRatingViewModel>> GetRating(Guid id);
        Task<IEnumerable<ExhibitionDetailViewModel>> GetAllExhibitions(string sortParams);
    }
}