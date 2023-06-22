﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FrogExhibitionDAL.Constants;
using FrogExhibitionDAL.Constants.ModelConstants.ExhibitionConstants;

namespace FrogExhibitionBLL.ViewModels.ExhibitionViewModels
{
    public class ExhibitionDetailViewModel
    {
        [Required]
        public Guid Id { get; set; }
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
