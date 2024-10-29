using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailo.Models
{
	public class OrderProduct
	{
		
		[ForeignKey("order")]
		public int OrderID { get; set; }
		public Order order { get; set; }

		[ForeignKey("product")]
		public int ProductID { get; set; }
		public Product product { get; set; }
		

	}
}

