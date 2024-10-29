using Mailo.Data;
using Mailo.Data.Enums;
using Mailo.IRepo;
using Mailo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Mailo.Repo
{
    public class CartRepo : ICartRepo
    {
        private readonly AppDbContext _db;
        public CartRepo(AppDbContext db)
        {
            _db = db;
        }
        public async Task<Order?> GetOrCreateCart(User user)
        {
            var cart = await _db.Orders
                .Include(o => o.OrderProducts) // Eagerly load OrderProducts
                .ThenInclude(op => op.product) // Eagerly load the actual Product associated with OrderProduct
                .FirstOrDefaultAsync(o => o.UserID == user.ID && o.OrderStatus == OrderStatus.New); // Assuming IsCompleted marks if it's an active cart

            return cart;
        }

        public async Task<List<Product>> GetProducts(Order order)
        {
            return await _db.OrderProducts.Where(op => op.OrderID == order.ID)
                .Select(op => op.product)
                .ToListAsync();
        }
        public async Task<OrderProduct> IsMatched(Order order, int id)
        {
            return await _db.OrderProducts.FirstOrDefaultAsync(op => op.OrderID == order.ID && op.ProductID == id);
        }

        //public async Task<Order> GetOrder(User user)
        //{
        //    Order? order = await _db.Orders.FirstOrDefaultAsync(o => new { o.UserID == user.ID && o.OrderStatus == OrderStatus.New });
        //        if (order == null)
        //        {
        //            order = new Order { OrderPrice = 0, OrderAddress = user.Address, UserID = user.ID
        //};
        //_db.Orders.Add(order);
        //            _db.SaveChanges();
        //        }

        //    return order;
        //}
        public async Task<Order> GetOrder(User user)
        {
            return await _db.Orders.FirstOrDefaultAsync(o => o.UserID == user.ID && o.OrderStatus == OrderStatus.New);

        }
        public async Task<List<Order>> GetOrders(User user)
        {

            return _db.Orders.Where(o => o.UserID == user.ID && (o.OrderStatus == OrderStatus.Pending || o.OrderStatus == OrderStatus.Shipped)).ToList();

        }
        public async Task<OrderProduct> ExistingCartItem(int productId, User user)
        {
            return await IsMatched(await GetOrder(user), productId);
        }
        public async void InsertToCart(int OrderId, int ProductId)
        {
            OrderProduct op = new OrderProduct
            {
                OrderID = OrderId,
                ProductID = ProductId
            };

            await _db.OrderProducts.AddAsync(op);
            _db.SaveChanges();
        }
        public async void DeleteFromCart(int OrderId, int ProductId)
        {
            OrderProduct op = new OrderProduct
            {
                OrderID = OrderId,
                ProductID = ProductId
            };
            _db.OrderProducts.Remove(op);
            _db.SaveChanges();
        }
    }
}