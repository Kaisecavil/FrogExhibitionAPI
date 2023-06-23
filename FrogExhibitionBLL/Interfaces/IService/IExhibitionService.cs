using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.ViewModels.ExhibitionViewModels;
using FrogExhibitionBLL.ViewModels.FrogViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IExhibitionService
    {
        Task<Guid> CreateExhibitionAsync(ExhibitionDtoForCreate exebition);
        Task DeleteExhibitionAsync(Guid id);
        Task<IEnumerable<ExhibitionDetailViewModel>> GetAllExhibitionsAsync();
        Task<ExhibitionDetailViewModel> GetExhibitionAsync(Guid id);
        Task UpdateExhibitionAsync(ExhibitionDtoForUpdate exebition);
        Task<IEnumerable<FrogRatingViewModel>> GetRatingAsync(Guid id);
        Task<IEnumerable<ExhibitionDetailViewModel>> GetAllExhibitionsAsync(string sortParams);
    }
}