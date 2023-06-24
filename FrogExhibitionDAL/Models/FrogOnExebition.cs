using FrogExhibitionDAL.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionDAL.Models
{
    public class FrogOnExhibition : BaseModel
    {
        [Required]
        public Guid ExhibitionId { get; set; }
        [Required]
        public Guid FrogId { get; set; }
        public virtual Exhibition Exhibition { get; set; } = null!;
        public virtual Frog Frog { get; set; } = null!;
        public virtual List<Vote> Votes { get; } = new();
    }
}
