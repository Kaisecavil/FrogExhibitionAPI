using FrogExhibitionDAL.Models.Base;

namespace FrogExhibitionDAL.Models
{
    public class FrogStarRating : BaseModel
    {
        public Guid FrogId { get; set; }
        public string ApplicationUserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual Frog Frog { get; set; } = null!;
    }
}
