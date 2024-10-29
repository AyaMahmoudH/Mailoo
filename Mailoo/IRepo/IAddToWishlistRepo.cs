using Mailo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mailo.IRepo
{
    public interface IAddToWishlistRepo
    {
        Task<List<Product>> GetProducts(int id);
        Task<Wishlist> ExistingWishlistItem(int id, int userId);
    }
}
