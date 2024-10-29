using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Mailo.Data.Enums;

namespace Mailo.Models
{
    public class Employee : User
    {
        //[Key]
        //public int ID { get; set; }
        //[MaxLength(20)]
        //public string FName { get; set; }
        //[MaxLength(20)]
        //public string LName { get; set; }
        //      public string? FullName { get; private set; }

        //      [MaxLength(12)]
        //public string PhoneNumber { get; set; }
        //[MaxLength(50)]
        //public string Email { get; set; }
        //[MaxLength(20)]
        //public string Password { get; set; }
        //[EnumDataType(typeof(Gender))]
        //public Gender Gender { get; set; }
        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; } = DateTime.Now;
        //public ICollection<Order>? orders { get; set; }

    }
}