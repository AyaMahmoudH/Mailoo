using Mailo.Models;

namespace Mailo.IRepo
{
    public interface ISearchRepo
    {
        Task<List<Product>> Search(string text);
    }
}
