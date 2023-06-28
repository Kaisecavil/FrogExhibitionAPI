using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.FrogStarRatingDTOs
{
    public class FrogStarRatingDtoForUpdate
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int Rating { get; set; }
        
        public string? Comment { get; set; }
    }
}
