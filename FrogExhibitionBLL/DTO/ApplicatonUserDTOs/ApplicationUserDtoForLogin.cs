using FrogExhibitionBLL.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.DTO.ApplicatonUserDTOs
{
    public class ApplicationUserDtoForLogin
    {
        [Required]
        [DefaultValue(DefaultValueConstants.AdminEmail)]
        public string Email { get; set; }
        [Required]
        [DefaultValue(DefaultValueConstants.BasicPassword)]
        public string Password { get; set; }
    }
}
