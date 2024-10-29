using Mailo.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mailoo.Models
{
    public class EditUserViewModel
    {
        public int ID { get; set; }

        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        [Required(ErrorMessage = "First name is required.")]
        [DisplayName("First Name")]
        public string FName { get; set; }

        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        public string LName { get; set; }

        [MaxLength(30, ErrorMessage = "Max 30 characters allowed.")]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [MaxLength(14)]
        [Required(ErrorMessage = "Phone Number is required.")]
        [DisplayName("Phone Number")]
       // [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012, or 015 and be 11 digits long.")]
        public string PhoneNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or Min 5 characters allowed.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Current Password is required.")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "New Password must be at least 6 characters long.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }

        [EnumDataType(typeof(Gender))]
        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [EnumDataType(typeof(UserType))]
        public UserType UserType { get; set; } = UserType.Client;

        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string Address { get; set; }
    }
}
