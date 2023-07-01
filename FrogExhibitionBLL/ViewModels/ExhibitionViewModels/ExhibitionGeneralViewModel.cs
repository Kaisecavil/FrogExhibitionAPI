using FrogExhibitionDAL.Constants.ModelConstants.ExhibitionConstants;
using System.ComponentModel;

namespace FrogExhibitionBLL.ViewModels.ExhibitionViewModels
{
    public class ExhibitionGeneralViewModel
    {
        public Guid Id { get; set; }

        [DefaultValue(ExhibitionDefaultValueConstants.NameDefaultValue)]
        public string Name { get; set; }

        public DateTime? Date { get; set; }

        [DefaultValue(ExhibitionDefaultValueConstants.CountryDefaultValue)]
        public string Country { get; set; }

        [DefaultValue(ExhibitionDefaultValueConstants.CityDefaultValue)]
        public string City { get; set; }

        [DefaultValue(ExhibitionDefaultValueConstants.StreetDefaultValue)]
        public string Street { get; set; }

        [DefaultValue(ExhibitionDefaultValueConstants.HouseDefaultValue)]
        public string House { get; set; }
    }
}
