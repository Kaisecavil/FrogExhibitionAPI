using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.VoteDtos
{
    public class VoteDtoForUpdate
    {
        [Required]
        public Guid Id { get; set; }
        //[Required]
        //public string ApplicationUserId { get; set; }
        [Required]
        public Guid FrogOnExhibitionId { get; set; }
    }
}
