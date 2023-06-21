using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FrogExhibitionBLL.DTO.FrogDTOs
{
    public class FrogDtoForCreate
    {
        [Required]
        [MinLength(3)]
        [DefaultValue("Lithobates")]
        public string Genus { get; set; }

        [Required]
        [MinLength(3)]
        [DefaultValue("L. catesbeianus")]
        public string Species { get; set; }

        [Required]
        [MinLength(3)]
        [DefaultValue("Green-brown")]
        public string Color { get; set; }

        [Required]
        [MinLength(3)]
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
        [DefaultValue(15f)]
        public float Size { get; set; }

        [Required]
        [Range(1, 1000)]
        [DefaultValue(350)]
        public float Weight { get; set; }

        [Required]
        [Range(1, Constants.MaxAge)]
        [DefaultValue(3)]
        public int CurrentAge { get; set; }
        [Required]
        [Range(1, Constants.MaxAge)]
        [DefaultValue(10)]
        public int MaxAge { get; set; }
        public string? Diet { get; set; }
        public string? Features { get; set; }
        public List<IFormFile>? Photos { get; set; }
    }
}
