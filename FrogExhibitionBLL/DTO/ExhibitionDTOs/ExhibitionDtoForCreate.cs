using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FrogExhibitionBLL.DTO.ExhibitionDTOs
{
    public class ExhibitionDtoForCreate
    {
        [Required]
        [MinLength(3)]
        [DefaultValue("Exhibition name")]
        public string Name { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        [MinLength(3)]
        [DefaultValue("Country")]
        public string Country { get; set; }
        [Required]
        [MinLength(3)]
        [DefaultValue("City")]
        public string City { get; set; }
        [Required]
        [MinLength(3)]
        [DefaultValue("Street")]
        public string Street { get; set; }
        [Required]
        [MinLength(3)]
        [DefaultValue("House")]
        public string House { get; set; }
    }
}
