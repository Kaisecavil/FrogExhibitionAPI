using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogExhibitionBLL.ViewModels.CommentViewModels
{
    public class CommentExcelReportViewModel
    {
        public string FrogGenus { get; set; }
        public string FrogSpecies { get; set; }
        public string FrogColor { get; set; }
        public string ExhibitionName { get; set; }
        public string ApplicationUserName { get; set; }
        public string Text { get; set; }
        public string CreationDate { get; set; }
    }
}
