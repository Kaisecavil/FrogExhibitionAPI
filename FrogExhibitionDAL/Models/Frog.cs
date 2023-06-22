using FrogExhibitionDAL.Constants.ModelConstants.FrogConstants;
using FrogExhibitionDAL.Constants;
using FrogExhibitionDAL.Enums;
using FrogExhibitionDAL.Models.Base;
using FrogExhibitionDAL.ValidationAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionDAL.Models
{
    [CurrentAgeFrog(ErrorMessage ="Current age can't be greater than max age")]
    public class Frog : BaseModel
    {
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
        public FrogSex Sex { get; set; }

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
        public virtual List<Exhibition> Exhibitions { get; } = new();
        public virtual List<FrogOnExhibition> FrogsOnExhibitions { get; } = new();
        public virtual List<FrogPhoto> FrogPhotos { get; } = new();

    }
}
