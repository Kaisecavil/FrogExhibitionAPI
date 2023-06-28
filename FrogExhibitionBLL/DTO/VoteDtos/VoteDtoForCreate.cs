using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.VoteDtos
{
    public class VoteDtoForCreate
    {
        //[Required]
        //public string ApplicationUserId { get; set; }
        [Required]
        public Guid FrogOnExhibitionId { get; set; }
    }
}
