using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mailo.Models
{
    public class EntityOrder
    {
        public int ID { get; set; }

        public string Name { get; set; }

     
        public string Mobile { get; set; }

        
        public string Email { get; set; }

     
       
    
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        
    }
}
