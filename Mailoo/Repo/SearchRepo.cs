using Mailo.Data;
using Mailo.IRepo;
using Mailo.Models;
using Microsoft.EntityFrameworkCore;

namespace Mailo.Repo
{
    public class SearchRepo :ISearchRepo
    {
        private readonly AppDbContext _db;
        public SearchRepo(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Product>> Search(string text)
        {
            
         return await _db.Products.Include(p => p.Name == text).ToListAsync();
        }
    }
}
