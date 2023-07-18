using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionDAL.Constants.ModelConstants.ExhibitionConstants;
using System.ComponentModel;

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
