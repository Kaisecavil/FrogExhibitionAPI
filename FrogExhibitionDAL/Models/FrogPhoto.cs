using FrogExhibitionDAL.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionDAL.Models
{
    public class FrogPhoto : BaseModel
    {
        [Required]
        public Guid FrogId { get; set; }
        public virtual Frog Frog { get; set; } = null!;
        [Required]
        public string PhotoPath { get; set; }
    }
}
