using FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs;
using FrogExhibitionBLL.ViewModels.FrogOnExhibitionViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IFrogOnExhibitionService
    {
        Task<FrogOnExhibitionDetailViewModel> CreateFrogOnExhibition(FrogOnExhibitionDtoForCreate frogOnExhibition);
        Task DeleteFrogOnExhibition(Guid id);
        Task<IEnumerable<FrogOnExhibitionDetailViewModel>> GetAllFrogOnExhibitions();
        Task<FrogOnExhibitionDetailViewModel> GetFrogOnExhibition(Guid id);
        Task UpdateFrogOnExhibition(Guid id, FrogOnExhibitionDtoForCreate frogOnExhibition);
    }
}