using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailo.Data.Enums;

namespace Mailo.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [DisplayName("Order Price")]

        public decimal OrderPrice { get; set; }
        [DisplayName("Delivery Fee")]

        public decimal DeliveryFee { get; set; }
        [DisplayName("Total Price")]

        public decimal TotalPrice { get; set; }

        [MaxLength(100)]
        [DisplayName("Order Address")]

        public string OrderAddress { get; set; }
        [DisplayName("Order Status")]

        public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
        [ForeignKey("user")]
        public int UserID { get; set; }
        public User? user { get; set; }
        [ForeignKey("employee")]
        public int? EmpID { get; set; }
        public Employee? employee { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
        public Payment? Payment { get; set; }

    }
}
