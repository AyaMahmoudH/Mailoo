using Mailo.Models;
using Mailoo.IRepo;

namespace Mailo.IRepo
{
    public interface IUnitOfWork : IDisposable
    {
        IBasicRepo<User> users { get; }
        IBasicRepo<Employee> employees { get; }
        IBasicRepo<Order> orders { get; }
        IBasicRepo<Product> products { get; }
        IBasicRepo<Wishlist> wishlists { get; }
        IBasicRepo<Payment> payments { get; }
        IBasicRepo<OrderProduct> orderProducts { get; }
        IBasicRepo<Contact> contacts { get; }
        IUserInfoRepo userRepo { get; }
        ISearchRepo search { get; }

        Task<int> CommitChangesAsync();

    }
}