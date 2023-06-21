using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FrogExhibitionBLL.ViewModels.ExhibitionViewModels
{
    public class ExhibitionDetailViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(FrogConstraintConstants.MinStrLength)]
        [DefaultValue("Exhibition name")]
        public string Name { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        [MinLength(FrogConstraintConstants.MinStrLength)]
        [DefaultValue("Country")]
        public string Country { get; set; }
        [Required]
        [MinLength(FrogConstraintConstants.MinStrLength)]
        [DefaultValue("City")]
        public string City { get; set; }
        [Required]
        [MinLength(FrogConstraintConstants.MinStrLength)]
        [DefaultValue("Street")]
        public string Street { get; set; }
        [Required]
        [MinLength(FrogConstraintConstants.MinStrLength)]
        [DefaultValue("House")]
        public string House { get; set; }
    }
}
