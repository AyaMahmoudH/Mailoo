using Mailo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mailo.IRepo
{
    public interface ICartRepo
    {

        Task<List<Product>> GetProducts(Order order);
        Task<OrderProduct> IsMatched(Order order, int id);
        Task<Order> GetOrder(User user);
        Task<OrderProduct> ExistingCartItem(int productId, User user);

        void InsertToCart(int OrderId, int ProductId);
        void DeleteFromCart(int OrderId, int ProductId);
        Task<Order> GetOrCreateCart(User user);
        Task<List<Order>> GetOrders(User user);


    }
}