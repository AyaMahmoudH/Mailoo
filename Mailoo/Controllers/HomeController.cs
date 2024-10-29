using Mailo.IRepo;
using Mailo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mailo.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            // _logger = logger;
            _unitOfWork = unitOfWork;


        }
        public async Task<ActionResult> Index()
        {
            var products = await _unitOfWork.products.GetAll();
            if (products != null)
            {
                return View(products);
            }
            else
            {
                return View();
            }
        }

    }
}