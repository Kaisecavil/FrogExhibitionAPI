using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs
{
    public class FrogOnExhibitionDtoForUpdate
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid ExhibitionId { get; set; }
        [Required]
        public Guid FrogId { get; set; }
    }
}
