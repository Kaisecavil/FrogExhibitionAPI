using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.CommentsDTOs
{
    public class CommentDtoForUpdate
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}
