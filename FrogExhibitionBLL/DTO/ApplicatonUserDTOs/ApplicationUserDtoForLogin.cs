using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.ApplicatonUserDTOs
{
    public class ApplicationUserDtoForLogin
    {
        [Required]
        [DefaultValue("Admin@mail.com")]
        public string Email { get; set; }
        [Required]
        [DefaultValue("P@ssw0rd")]
        public string Password { get; set; }
    }
}
