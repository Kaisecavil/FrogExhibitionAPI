using FrogExhibitionDAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FrogExhibitionDAL.Models
{
    public class Exhibition : BaseModel
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

        public virtual List<Frog> Frogs { get; set; } = new();
        public virtual List<FrogOnExhibition> FrogsOnExhibitions { get; set; } = new();
    }
}
