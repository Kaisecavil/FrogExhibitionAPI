using FrogExhibitionBLL.ViewModels.CommentViewModels;
using FrogExhibitionBLL.ViewModels.VoteViewModels;

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
