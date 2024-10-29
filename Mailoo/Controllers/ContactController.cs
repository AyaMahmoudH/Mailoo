using Mailo.Data;
using Mailo.IRepo;
using Mailo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mailo.Controllers
{

    public class ContactController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public ContactController(AppDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index_U()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Index_A()
        {
            var contacts = _unitOfWork.contacts.GetAll();
            return View(contacts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index_U(Contact model)
        {
            User? user = await _unitOfWork.userRepo.GetUser(User.Identity.Name);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                model.userId = user.ID; 
                _db.Contacts.Add(model);
                _db.SaveChanges();
                TempData["SuccessContact"] = "Your message has been sent successfully!";
                return RedirectToAction("Index_U");
            }
            return View(model);
        }
        public IActionResult Complaints()
        {
            var contacts = _db.Contacts.Include(c=>c.user).ToList();
            return View(contacts);
        }

    }
}