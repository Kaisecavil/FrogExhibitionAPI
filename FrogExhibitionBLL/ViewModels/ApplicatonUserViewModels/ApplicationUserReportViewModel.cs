using FrogExhibitionBLL.ViewModels.CommentViewModels;
using FrogExhibitionBLL.ViewModels.VoteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels
{
    public class ApplicationUserReportViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<CommentReportViewModel> Comments { get; set; }
        public List<VoteReportViewModel> Votes { get; set; }
    }
}
