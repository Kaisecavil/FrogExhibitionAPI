using FrogExhibitionDAL.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionDAL.Models
{
    public class Vote : BaseModel 
    {
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public Guid FrogOnExhibitionId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual FrogOnExhibition FrogOnExhibition { get; set; } = null!;
    }
}
