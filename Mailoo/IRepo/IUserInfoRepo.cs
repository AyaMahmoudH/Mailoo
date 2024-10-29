using Mailo.Models;

namespace Mailoo.IRepo
{
    public interface IUserInfoRepo
    {
        Task<User> GetUser(string? Email);
    }
}
