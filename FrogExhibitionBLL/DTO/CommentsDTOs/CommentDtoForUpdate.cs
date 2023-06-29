using FrogExhibitionDAL.Constants.ModelConstants.CommentConstants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.CommentsDTOs
{
    public class CommentDtoForUpdate
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        [MinLength(CommentsConstaraintConstants.MinTextLength)]
        [MaxLength(CommentsConstaraintConstants.MaxTextLength)]
        [DefaultValue(CommentsDefaultValueConstants.TextDefaultValue)]
        public string Text { get; set; }
    }
}
