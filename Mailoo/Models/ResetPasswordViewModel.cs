using System.ComponentModel.DataAnnotations;

namespace Mailoo.Models
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
