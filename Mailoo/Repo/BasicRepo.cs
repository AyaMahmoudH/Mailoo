using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mailo.Data;
using Mailo.IRepo;
using Mailo.Models;

namespace Mailo.Repo
{
	public class BasicRepo<T> : IBasicRepo<T> where T : class
	{
		protected readonly AppDbContext _db;

        public BasicRepo(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<T>> GetAll()
		{
			//modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())
			return await _db.Set<T>().ToListAsync();
		}

		public async Task<T> GetByID(int id)
		{
			return await _db.Set<T>().FindAsync(id);
		}
		
		public async void Insert(T entity)
		{
			await _db.Set<T>().AddAsync(entity);
			_db.SaveChanges();
		}
		public  void Update(T entity)
		{
			_db.Set<T>().Update(entity);
			_db.SaveChanges();
		}
		public  void Delete(T entity)
		{
			_db.Set<T>().Remove(entity);
			_db.SaveChanges();
		}
        
    }
}
