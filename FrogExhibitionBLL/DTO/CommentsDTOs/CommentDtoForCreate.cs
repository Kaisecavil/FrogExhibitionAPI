using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.CommentsDTOs
{
    public class CommentDtoForCreate
    {
        [Required]
        public Guid FrogOnExhibitionId { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}
