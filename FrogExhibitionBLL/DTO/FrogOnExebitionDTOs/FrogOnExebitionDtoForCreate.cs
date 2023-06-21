using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs
{
    public class FrogOnExhibitionDtoForCreate
    {
        [Required]
        public Guid ExhibitionId { get; set; }
        [Required]
        public Guid FrogId { get; set; }
    }
}
