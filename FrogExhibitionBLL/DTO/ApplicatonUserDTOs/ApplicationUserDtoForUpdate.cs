using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.ApplicatonUserDTOs
{
    public class ApplicationUserDtoForUpdate
    {
        [Required]
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? KnowledgeLevel { get; set; }
    }
}
