using FrogExhibitionBLL.DTO.ExhibitionDTOs;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface IExhibitionService
    {
        Task<ExhibitionDtoDetail> CreateExhibition(ExhibitionDtoForCreate exebition);
        Task DeleteExhibition(Guid id);
        Task<IEnumerable<ExhibitionDtoDetail>> GetAllExhibitions();
        Task<ExhibitionDtoDetail> GetExhibition(Guid id);
        Task UpdateExhibition(Guid id, ExhibitionDtoForCreate exebition);
        Task<IEnumerable<FrogDtoRating>> GetRating(Guid id);
        Task<IEnumerable<ExhibitionDtoDetail>> GetAllExhibitions(string sortParams);
    }
}