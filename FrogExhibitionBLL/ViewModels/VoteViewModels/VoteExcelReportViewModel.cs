using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogExhibitionBLL.ViewModels.VoteViewModels
{
    public class VoteExcelReportViewModel
    {
        public string ApplicationUserName { get; set; }
        public string FrogGenus { get; set; }
        public string FrogSpecies { get; set; }
        public string FrogColor { get; set; }
        public string ExhibitionName { get; set; }
        public int FrogsPlaceOnExhibition { get; set; }
    }
}
