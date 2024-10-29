using System.Collections.Generic;
namespace Mailo.IRepo
{
	public interface IBasicRepo<T> where T : class
	{
		Task<IEnumerable<T>> GetAll();
		Task<T> GetByID(int id);
		void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
	}
}
