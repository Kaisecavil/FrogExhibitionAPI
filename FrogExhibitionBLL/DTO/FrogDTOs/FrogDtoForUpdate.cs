﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using FrogExhibitionDAL.Constants.ModelConstants.FrogConstants;
using FrogExhibitionDAL.Constants;
using FrogExhibitionBLL.ValidationAttributes;
using FrogExhibitionBLL.Constants;

namespace FrogExhibitionBLL.DTO.FrogDTOs
{
    [CurrentAgeFrog(ErrorMessage = ErrorMessages.CurrentFrogAgeValidationError)]
    public class FrogDtoForUpdate
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(FrogDefaultValueConstants.FrogGenusDefaultValue)]
        public string Genus { get; set; }

        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(FrogDefaultValueConstants.FrogSpeciesDefaultValue)]
        public string Species { get; set; }

        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(FrogDefaultValueConstants.FrogColorDefaultValue)]
        public string Color { get; set; }

        [Required]
        [MinLength(GeneralConstants.MinStrLength)]
        [DefaultValue(FrogDefaultValueConstants.FrogHabitatDefaultValue)]
        public string Habitat { get; set; }

        [Required]
        [DefaultValue(FrogDefaultValueConstants.FrogPosisonousDefaultValue)]
        public bool Poisonous { get; set; }

        [Required]
        [DefaultValue(FrogDefaultValueConstants.FrogSexDefaultValue)]
        public string Sex { get; set; }

        [Required]
        [DefaultValue(FrogDefaultValueConstants.FrogHouseKeepableDefaultValue)]
        public bool HouseKeepable { get; set; }

        [Required]
        [Range(FrogConstraintConstants.MinFrogSize, FrogConstraintConstants.MaxFrogSize)]
        [DefaultValue(FrogDefaultValueConstants.FrogSizeDefaultValue)]
        public float Size { get; set; }

        [Required]
        [Range(FrogConstraintConstants.MinFrogWeight, FrogConstraintConstants.MaxFrogWeight)]
        [DefaultValue(FrogDefaultValueConstants.FrogWeightDefaultValue)]
        public float Weight { get; set; }

        [Required]
        [Range(FrogConstraintConstants.MinFrogAge, FrogConstraintConstants.MaxFrogAge)]
        [DefaultValue(FrogConstraintConstants.MinFrogAge)]
        public int CurrentAge { get; set; }
        [Required]
        [Range(FrogConstraintConstants.MinFrogAge, FrogConstraintConstants.MaxFrogAge)]
        [DefaultValue(FrogDefaultValueConstants.FrogMaxAgeDefaultValue)]
        public int MaxAge { get; set; }
        public string? Diet { get; set; }
        public string? Features { get; set; }
        public List<IFormFile>? Photos { get; set; }
    }
}
