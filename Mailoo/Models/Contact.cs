using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mailo.Models
{
    public class Contact
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
        [ForeignKey("user")]
        public int? userId { get; set; }
        public User? user { get; set; }

    }
}
