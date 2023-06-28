using FrogExhibitionBLL.DTO.CommentsDTOs;
using FrogExhibitionBLL.ViewModels.CommentViewModels;

namespace FrogExhibitionBLL.Interfaces.IService
{
    public interface ICommentService
    {
        Task<Guid> CreateCommentAsync(CommentDtoForCreate comment);
        Task DeleteCommentAsync(Guid id);
        Task<IEnumerable<CommentGeneralViewModel>> GetAllCommentsAsync();
        Task<CommentGeneralViewModel> GetCommentAsync(Guid id);
        Task UpdateCommentAsync(CommentDtoForUpdate comment);
    }
}