using Mailo.Data;
using Mailo.Data.Enums;
using Mailo.IRepo;
using Mailo.Models;
using Mailo.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Mailo.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepo _order;
        private readonly IUnitOfWork _unitOfWork;
        public CartController(ICartRepo order, IUnitOfWork unitOfWork)
        {
            _order = order;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            User? user = await _unitOfWork.userRepo.GetUser(User.Identity.Name);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("Login", "Account");
            }

            Order? cart = await _order.GetOrCreateCart(user);
            if (cart == null || cart.OrderProducts == null)
            {
                return View("EmptyCart");
                //TempData["ErrorMessage"] = "Cart is empty";
                //return BadRequest(TempData["ErrorMessage"]);

            }
            return View(cart.OrderProducts.Select(op => op.product).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart()
        {
            User? user = await _unitOfWork.userRepo.GetUser(User.Identity.Name);
            var cart = await _order.GetOrCreateCart(user);
            if (cart != null)
            {
                cart.OrderProducts.Clear();
                _unitOfWork.orders.Delete(cart);
            }
            else
            {
                TempData["ErrorMessage"] = "Cart is already empty";
                return BadRequest(TempData["ErrorMessage"]);

            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            User? user = await _unitOfWork.userRepo.GetUser(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found";
                return BadRequest(TempData["ErrorMessage"]);

            }

            var cart = await _order.GetOrCreateCart(user);

            if (cart == null)
            {
                cart = new Order
                {
                    UserID = user.ID,
                    OrderPrice = product.TotalPrice,
                    OrderAddress = user.Address,
                    OrderProducts = new List<OrderProduct>()
                };

                _unitOfWork.orders.Insert(cart);
                await _unitOfWork.CommitChangesAsync();
                cart.OrderProducts.Add(new OrderProduct
                {
                    ProductID = product.ID,
                    OrderID = cart.ID
                });

                _unitOfWork.orders.Update(cart);
                await _unitOfWork.CommitChangesAsync();
            }
            else
            {
                OrderProduct? existingOrderProduct = cart.OrderProducts
                    .Where(op => op.ProductID == product.ID)
                    .FirstOrDefault();

                if (existingOrderProduct != null)
                {
                    TempData["ErrorMessage"] = "Product is already in cart";
                    return BadRequest(TempData["ErrorMessage"]);

                }
                else
                {
                    cart.OrderPrice += product.TotalPrice;

                    cart.OrderProducts.Add(new OrderProduct
                    {
                        ProductID = product.ID,
                        OrderID = cart.ID
                    });

                    _unitOfWork.orders.Update(cart);
                    await _unitOfWork.CommitChangesAsync();
                }
            }

            return RedirectToAction("Index_U", "User");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            User? user = await _unitOfWork.userRepo.GetUser(User.Identity.Name);
            var cart = await _order.GetOrCreateCart(user);
            if (cart == null)
            {
                TempData["ErrorMessage"] = "Cart is empty";
                return BadRequest(TempData["ErrorMessage"]);
            }
            else
            {
                var orderProduct = cart.OrderProducts.FirstOrDefault(op => op.ProductID == productId);
                if (orderProduct != null)
                {
                    var product = await _unitOfWork.products.GetByID(productId);
                    cart.OrderPrice -= product.TotalPrice;
                    cart.OrderProducts.Remove(orderProduct);
                    if (cart.OrderProducts == null || !cart.OrderProducts.Any())
                    {
                        await ClearCart();
                    }
                    await _unitOfWork.CommitChangesAsync();
                }
                else
                {
                    TempData["ErrorMessage"] = "Product not found";
                    return BadRequest(TempData["ErrorMessage"]);


                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewOrder()
        {
            User? user = await _unitOfWork.userRepo.GetUser(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var existingOrderItem = await _order.GetOrCreateCart(user);

            if (existingOrderItem == null || (existingOrderItem.OrderStatus != OrderStatus.New))
            {
                TempData["ErrorMessage"] = "Cart is already ordered";
                return BadRequest(TempData["ErrorMessage"]);
            }
            if (existingOrderItem.OrderProducts != null && existingOrderItem.OrderProducts.Any())
            {
                var products = existingOrderItem.OrderProducts.Where(op => op.OrderID == existingOrderItem.ID)
                    .Select(op => op.product)
                    .ToList();
                foreach (var product in products)
                {
                    product.Quantity -= 1;
                }

                existingOrderItem.OrderStatus = OrderStatus.Pending;
                _unitOfWork.orders.Update(existingOrderItem);
                TempData["Success"] = "Cart Has Been Ordered Successfully";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Cart is empty";
            return BadRequest(TempData["ErrorMessage"]);
        }

        public async Task<IActionResult> GetOrders()
        {
            User? user = await _unitOfWork.userRepo.GetUser(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var orders = await _order.GetOrders(user);
            if (orders != null)
            {
                return View(orders);
            }
            else
            {
                TempData["ErrorMessage"] = "Cart is already empty";
                return BadRequest(TempData["ErrorMessage"]);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(Order order)
        {
            User? user = await _unitOfWork.userRepo.GetUser(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (order != null)
            {
                var orderProducts = await _unitOfWork.orderProducts.GetAll();
                var orderP = orderProducts.Where(op => op.OrderID == order.ID);

                if (orderP.Any())
                {
                    foreach (var orderProduct in orderP)
                    {
                        _unitOfWork.orderProducts.Delete(orderProduct);
                    }
                }
                _unitOfWork.orders.Delete(order);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Cart is already empty";
                return BadRequest(TempData["ErrorMessage"]);
            }
        }
    }
}