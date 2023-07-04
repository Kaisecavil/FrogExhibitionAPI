using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionDAL.Constants.ModelConstants.ExhibitionConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogExhibitionBLL.ViewModels.ExhibitionViewModels
{
    public class ExhibitionReportViewModel
    {
        [DefaultValue(ExhibitionDefaultValueConstants.NameDefaultValue)]
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public List<FrogReportViewModel> Frogs { get; set; }
    }
}
