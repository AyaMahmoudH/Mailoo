using Mailo.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailo.Models
{
    [NotMapped]
    public class Product
    {
        public int ID { get; set; }
		[MaxLength(15)]
		public string Name { get; set; }
		public string? Description { get; set; }
        public Product_Categories Category { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        [MaxLength(50)]
        public string AdditionDate { get; set; } = DateTime.Now.ToString();
        public decimal Discount { get; set; } = 0;
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
        //	public ICollection<OrderProduct>? OrderProducts { get; set; }
        public ICollection<Wishlist>? wishlists { get; set; }
       
        [NotMapped]
        public IFormFile? clientFile { get; set; }
        public byte[]? dbImage { get; set; }
        [NotMapped]
        public string? imageSrc
        {
            get
            {
                if (dbImage != null)
                {
                    string base64String = Convert.ToBase64String(dbImage, 0, dbImage.Length);
                    return "data:image/jpg;base64," + base64String;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
            }
        }
    }  
}
