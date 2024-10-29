using Mailo.Data.Enums;
using Mailo.Data;
using Mailo.IRepo;
using Mailo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Mailoo.Models;
using Microsoft.AspNetCore.Identity;

namespace Mailo.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
  
        public UserController(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
         
        }


        #region Admin 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index_A()
        {
            return View(await _unitOfWork.products.GetAll());
        }
        public async Task<IActionResult> New()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> New(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.clientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    product.clientFile.CopyTo(stream);
                    product.dbImage = stream.ToArray();
                }
                _unitOfWork.products.Insert(product);
                TempData["Success"] = "product Has Been Added Successfully";
                return RedirectToAction("Index_A");
            }
            else
                return View(product);
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                return View(await _unitOfWork.products.GetByID(id));
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id != 0)
            {
                var product = await _unitOfWork.products.GetByID(id);
                if (product != null)
                {
                    _unitOfWork.products.Delete(product);
                    TempData["Success"] = "product Has Been Deleted Successfully";
                    return RedirectToAction("Index_A");
                }
            }
            return NotFound();

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != 0)
            {
                // Get the product by ID
                var product = await _unitOfWork.products.GetByID(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ID);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Handle the file upload, if there is a file
            if (product.clientFile != null && product.clientFile.Length > 0)
            {
                // Save the uploaded file to a specific folder, e.g., wwwroot/images
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", product.clientFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.clientFile.CopyToAsync(stream);
                }

                // Update the image path in the existing product
                existingProduct.imageSrc = "~/images/" + product.clientFile.FileName;
            }

            if (ModelState.IsValid)
            {
                // Update only the necessary fields in the existing product
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                // Update other fields as needed

                // Now update the existing product in the database
                _unitOfWork.products.Update(existingProduct);

                TempData["Success"] = "Product Has Been Updated Successfully";
                return RedirectToAction("Index_A");
            }

            return View(product);
        }

        #endregion



        #region Client
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Index_U()
        {
            return View(await _unitOfWork.products.GetAll());
        }

        #endregion




       

    }
}
