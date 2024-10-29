using Mailo.Models;
using Microsoft.AspNetCore.Mvc;
using Mailo.Repo;
using Mailo.IRepo;
using Mailo.Data;
using Mailo.Data.Enums;
using Microsoft.EntityFrameworkCore;
namespace Mailo.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
       
        public ProductController(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.products.GetAll());
        }
        #region Aya
        public async Task<IActionResult> Getpants()
        {
            var products = _context.Products.Where(p => p.Category == Product_Categories.Pants);
            return View(products);
        }
        public async Task<IActionResult> Gethoddies()
        {
            var products = _context.Products.Where(p => p.Category == Product_Categories.hoodi);
            return View(products);
        }
        #endregion
       
    }
}












