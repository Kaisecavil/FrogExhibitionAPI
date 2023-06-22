using FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs;
using FrogExhibitionBLL.ViewModels.FrogOnExhibitionViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IFrogOnExhibitionService
    {
        Task<Guid> CreateFrogOnExhibitionAsync(FrogOnExhibitionDtoForCreate frogOnExhibition);
        Task DeleteFrogOnExhibitionAsync(Guid id);
        Task<IEnumerable<FrogOnExhibitionDetailViewModel>> GetAllFrogOnExhibitionsAsync();
        Task<FrogOnExhibitionDetailViewModel> GetFrogOnExhibitionAsync(Guid id);
        Task UpdateFrogOnExhibitionAsync(Guid id, FrogOnExhibitionDtoForCreate frogOnExhibition);
    }
}