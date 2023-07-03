using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.VoteDtos
{
    public class VoteDtoForCreate
    {
        [Required]
        public Guid FrogOnExhibitionId { get; set; }
    }
}
