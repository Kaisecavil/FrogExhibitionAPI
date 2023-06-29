using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FrogExhibitionDAL.Constants.ModelConstants.FrogStarRatingConstants;

namespace FrogExhibitionBLL.DTO.FrogStarRatingDTOs
{
    public class FrogStarRatingDtoForCreate
    {
        [Required]
        public Guid FrogId { get; set; }
        [Required]
        [Range(FrogStarRatingConstraintConstants.MinRating, FrogStarRatingConstraintConstants.MaxRating)]
        [DefaultValue(FrogStarRatingDefaultValueConstants.RatingDefaultValue)]
        public int Rating { get; set; }

        [DefaultValue(FrogStarRatingDefaultValueConstants.CommentDefaultValue)]
        public string? Comment { get; set; }

    }
}
