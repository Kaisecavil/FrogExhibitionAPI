using System.ComponentModel;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionDAL.Constants.ModelConstants.ExhibitionConstants;

namespace FrogExhibitionBLL.ViewModels.ExhibitionViewModels
{
    public class ExhibitionDetailViewModel
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

        public List<FrogExhibitionViewModel> Frogs { get; set; }
    }
}
