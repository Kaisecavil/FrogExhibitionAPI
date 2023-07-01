﻿using FrogExhibitionDAL.Constants.ModelConstants.FrogConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogExhibitionBLL.ViewModels.FrogViewModels
{
    public class FrogExcelReportViewModel
    {
        public Guid Id { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogGenusDefaultValue)]
        public string Genus { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogSpeciesDefaultValue)]
        public string Species { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogColorDefaultValue)]
        public string Color { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogHabitatDefaultValue)]
        public string Habitat { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogPosisonousDefaultValue)]
        public bool Poisonous { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogSexStringDefaultValue)]
        public string Sex { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogHouseKeepableDefaultValue)]
        public bool HouseKeepable { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogSizeDefaultValue)]
        public float Size { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogWeightDefaultValue)]
        public float Weight { get; set; }

        [DefaultValue(FrogConstraintConstants.MinFrogAge)]
        public int CurrentAge { get; set; }

        [DefaultValue(FrogDefaultValueConstants.FrogMaxAgeDefaultValue)]
        public int MaxAge { get; set; }
        public int VotesCount { get; set; }
        public int VotesSum { get; set; }
        public int CommentsCount { get; set; }
        public double AverageRating { get; set; }
    }
}
