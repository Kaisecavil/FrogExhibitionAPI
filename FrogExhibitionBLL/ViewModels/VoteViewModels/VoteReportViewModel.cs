using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogExhibitionBLL.ViewModels.VoteViewModels
{
    public class VoteReportViewModel
    {
        public string ApplicationUserName { get; set; }
        public string FrogGenus { get; set; }
        public string FrogSpecies { get; set; }
        public string FrogColor { get; set; }
        public string ExhibitionName { get; set; }
        public string ExhibitionDate { get; set; }
        public int FrogsPlaceOnExhibition { get; set; }
    }
}
