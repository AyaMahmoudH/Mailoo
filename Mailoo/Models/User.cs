using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Mailo.Data.Enums;

namespace Mailo.Models
{
    public class User
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

        public string FullName { get; set; }

        [MaxLength(30, ErrorMessage = "Max 30 characters allowed.")]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [MaxLength(14)]
        [Required(ErrorMessage = "Phone Number is required.")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [EnumDataType(typeof(Gender))]
        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [EnumDataType(typeof(UserType))]
        public UserType UserType { get; set; } = UserType.Client;

        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string Address { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public string PasswordResetToken { get; set; } = Guid.NewGuid().ToString();
        public ICollection<Contact>? Message { get; set; }
        public ICollection<Wishlist>? wishlist { get; set; }
        public ICollection<Order>? orders { get; set; }
        public ICollection<Payment>? payment { get; set; }
    }
}