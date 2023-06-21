using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FrogExhibitionBLL.ViewModels.FrogViewModels
{
    public class FrogRatingViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(ModelConstraintConstants.MinStrLength)]
        [DefaultValue("Lithobates")]
        public string Genus { get; set; }

        [Required]
        [MinLength(ModelConstraintConstants.MinStrLength)]
        [DefaultValue("L. catesbeianus")]
        public string Species { get; set; }

        [Required]
        [MinLength(ModelConstraintConstants.MinStrLength)]
        [DefaultValue("Green-brown")]
        public string Color { get; set; }

        [Required]
        [MinLength(ModelConstraintConstants.MinStrLength)]
        [DefaultValue("North America")]
        public string Habitat { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Poisonous { get; set; }

        [Required]
        [ValidStrings(new string[] { "Male", "Female", "Hermaphrodite" }, ErrorMessage = "Valid options: Male, Female, Hermaphrodite")]
        [DefaultValue("Male")]
        public string Sex { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool HouseKeepable { get; set; }

        [Required]
        [Range(1, 100)]
        [DefaultValue(15.5f)]
        public float Size { get; set; }

        [Required]
        [Range(1, 1000)]
        [DefaultValue(ModelConstraintConstants.MinStrLength50)]
        public float Weight { get; set; }

        [Required]
        [Range(1, Constants.MaxAge)]
        [DefaultValue(ModelConstraintConstants.MinStrLength)]
        public int CurrentAge { get; set; }
        [Required]
        [Range(1, Constants.MaxAge)]
        [DefaultValue(10)]
        public int MaxAge { get; set; }
        public List<string> PhotoPaths { get; set; }
        [Required]
        public int VotesCount { get; set; }
    }
}
