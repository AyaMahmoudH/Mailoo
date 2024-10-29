using Mailo.Data;
using Mailo.IRepo;
using Mailo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Mailo.Repo
{
    public class AddToWishlistRepo : IAddToWishlistRepo
    {
        private readonly AppDbContext _db;
        public AddToWishlistRepo(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Product>> GetProducts(int id)
        {
            var products = _db.Wishlists.Where(w => w.UserID == id)
                .Select(w => w.product)
                .ToListAsync();
            return await products;
        }
        public async Task<Wishlist> ExistingWishlistItem(int id,int userId) {
            
            return await _db.Wishlists.FirstOrDefaultAsync(w => w.UserID == userId && w.ProductID == id);
        }

        
    }
}
