using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mailo.Models
{
    public class LoginViewModel
    {

        [MaxLength(30, ErrorMessage = "Max 30 characters allowed.")]
        [Required(ErrorMessage = "Username or Email is required.")]
        [DisplayName("UserName or Email")]
        public string UsernameOrEmail { get; set; }

        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or Min 5 characters allowed.")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}
