using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.ViewModels.FrogOnExhibitionViewModels
{
    public class FrogOnExhibitionDetailViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ExhibitionId { get; set; }
        [Required]
        public Guid FrogId { get; set; }
    }
}
