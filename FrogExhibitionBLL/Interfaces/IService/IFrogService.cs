using FrogExhibitionBLL.DTO.FrogDTOs;
using FrogExhibitionBLL.ViewModels.FrogViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IFrogService
    {
        public Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogsAsync();
        public Task<IEnumerable<FrogGeneralViewModel>> GetAllFrogsAsync(string sortParams);
        public Task<FrogDetailViewModel> GetFrogAsync(Guid id);
        public Task<Guid> CreateFrogAsync(FrogDtoForCreate frog);
        public Task DeleteFrogAsync(Guid id);
        public Task UpdateFrogAsync(FrogDtoForUpdate frog);
    }
}
