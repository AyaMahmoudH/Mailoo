using Mailo.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mailo.Models
{
    public class RegistrationViewModel
    {
        [Key]
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
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "010, 011, 012, or 015, start with 11 digit numbers. ")]
        public string PhoneNumber { get; set; }
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        [Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage ="Please Enter Valid Email")]

        public string Email { get; set; }
        [StringLength(20, MinimumLength =5, ErrorMessage = "Max 20 or Min 5 characters allowed.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
//        [RegularExpression("^(?=.[a-z])(?=.[A-Z])(?=.\\d)(?=.[@$!%?&])[A-Za-z\\d@$!%?&]{8,}$",
//ErrorMessage = "Password must be at least 8 characters long, contain one uppercase, one lowercase, one number, and one special character.")]

        public string Password { get; set; }

        [Compare("Password",ErrorMessage="Please confirm your password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [EnumDataType(typeof(Gender))]
        [Required(ErrorMessage = "Gender is required.")]

        public Gender Gender { get; set; }
        [EnumDataType(typeof(UserType))]
        public UserType UserType { get; set; } = UserType.Client;
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public ICollection<Wishlist>? wishlist { get; set; }
        public ICollection<Order>? orders { get; set; }
        public ICollection<Payment>? payment { get; set; }
    }
}
