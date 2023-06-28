using FrogExhibitionDAL.Models.Base;

namespace FrogExhibitionDAL.Models
{
    public class Comment : BaseModel
    {
        public Guid FrogOnExhibitionId { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual FrogOnExhibition FrogOnExhibition { get; set; } = null!;
    }
}
