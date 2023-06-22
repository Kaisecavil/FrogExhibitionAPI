using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FrogExhibitionDAL.Constants.ModelConstants.ExhibitionConstants;
using FrogExhibitionDAL.Constants;

namespace FrogExhibitionBLL.DTO.ExhibitionDTOs
{
    public class ExhibitionDtoForCreate
    {
        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(ExhibitionDefaultValueConstants.NameDefaultValue)]
        public string Name { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(ExhibitionDefaultValueConstants.CountryDefaultValue)]
        public string Country { get; set; }
        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(ExhibitionDefaultValueConstants.CityDefaultValue)]
        public string City { get; set; }
        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(ExhibitionDefaultValueConstants.StreetDefaultValue)]
        public string Street { get; set; }
        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(ExhibitionDefaultValueConstants.HouseDefaultValue)]
        public string House { get; set; }
    }
}
