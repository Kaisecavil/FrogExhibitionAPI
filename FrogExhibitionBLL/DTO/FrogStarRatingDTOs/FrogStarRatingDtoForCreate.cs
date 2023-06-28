using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.FrogStarRatingDTOs
{
    public class FrogStarRatingDtoForCreate
    {
        [Required]
        public Guid FrogId { get; set; }
        [Required]
        public int Rating { get; set; }

        public string? Comment { get; set; }

    }
}
