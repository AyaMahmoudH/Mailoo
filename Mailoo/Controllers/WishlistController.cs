using Mailo.Data;
using Mailo.IRepo;
using Mailo.Models;
using Mailo.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Mailo.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IAddToWishlistRepo _wishlist;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;
        public WishlistController(AppDbContext db, IAddToWishlistRepo wishlist, IUnitOfWork unitOfWork)
        {
            _wishlist = wishlist;
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            User? user = _db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var wishlistItems = _db.Wishlists
                .Where(w => w.UserID == user.ID)
                .Select(w => w.ProductID) // Get the ItemIds from the wishlist
                .ToList();
                if (wishlistItems == null || wishlistItems.Count == 0 || !wishlistItems.Any())
                {
                    return View("EmptyCart");
                }
                // Fetch products based on ItemIds
                var products = _db.Products
                    .Where(p => wishlistItems.Contains(p.ID))
                    .ToList();
                return View(products);
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }


        }



        [HttpPost]
        public async Task<IActionResult> AddtoWishlist(Product product)
        {
            var productwishlist = await _db.Products.FirstOrDefaultAsync(p => p.ID == product.ID);

            User? user = _db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var productid = await _db.Products.FindAsync(product.ID);
                if (product != null)
                {
                    var existingWishlistEntry = await _db.Wishlists.FirstOrDefaultAsync(w => w.UserID == user.ID && w.ProductID == product.ID);

                    if (existingWishlistEntry == null)
                    {
                        var wishlistEntry = new Wishlist
                        {
                            UserID = user.ID,
                            ProductID = product.ID,
                            AdditionDate = DateTime.Now
                        };

                        _db.Wishlists.Add(wishlistEntry);
                        await _db.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Product added to wishlist successfully.";
                    }
                    else
                    {
                        TempData["WarningMessage"] = "The product is already in the wishlist.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Product not found.";

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
            return RedirectToAction("Index_U", "User");

        }



        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(Product product)
        {

            var productwishlist = await _db.Products.FirstOrDefaultAsync(p => p.ID == product.ID);

            User? user = _db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var productid = await _db.Products.FindAsync(product.ID);
                if (product != null)
                {
                    var wishlistEntry = await _db.Wishlists.FirstOrDefaultAsync(w => w.UserID == user.ID && w.ProductID == product.ID);

                    if (wishlistEntry != null)
                    {
                        _db.Wishlists.Remove(wishlistEntry);
                        await _db.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Product removed from wishlist successfully.";
                    }
                    else
                    {
                        TempData["WarningMessage"] = "The product is already in the wishlist.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Product not found.";

                }


            }

            return RedirectToAction("Index");

        }
    }
}