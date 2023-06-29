using FrogExhibitionDAL.Constants.ModelConstants.FrogStarRatingConstants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.FrogStarRatingDTOs
{
    public class FrogStarRatingDtoForUpdate
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(FrogStarRatingConstraintConstants.MinRating, FrogStarRatingConstraintConstants.MaxRating)]
        [DefaultValue(FrogStarRatingDefaultValueConstants.RatingDefaultValue)]
        public int Rating { get; set; }

        [DefaultValue(FrogStarRatingDefaultValueConstants.CommentDefaultValue)]
        public string? Comment { get; set; }
    }
}
