using FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IFrogOnExhibitionService
    {
        Task<FrogOnExhibitionDtoDetail> CreateFrogOnExhibition(FrogOnExhibitionDtoForCreate frogOnExhibition);
        Task DeleteFrogOnExhibition(Guid id);
        Task<IEnumerable<FrogOnExhibitionDtoDetail>> GetAllFrogOnExhibitions();
        Task<FrogOnExhibitionDtoDetail> GetFrogOnExhibition(Guid id);
        Task UpdateFrogOnExhibition(Guid id, FrogOnExhibitionDtoForCreate frogOnExhibition);
    }
}