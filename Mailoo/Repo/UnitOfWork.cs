using Mailo.Data;
using Mailo.IRepo;
using Mailo.Models;
using Mailoo.IRepo;
using Mailoo.Repo;

namespace Mailo.Repo
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
			users=new BasicRepo<User>(_db);
			employees=new BasicRepo<Employee>(_db);
			orders=new BasicRepo<Order>(_db);
			products=new BasicRepo<Product>(_db);
			wishlists=new BasicRepo<Wishlist>(_db);
			payments=new BasicRepo<Payment>(_db);
            orderProducts=new BasicRepo<OrderProduct>(_db);
            search =new SearchRepo(_db);
            userRepo = new UserInfoRepo(_db);
        }
		public IBasicRepo<User> users { get; private set; }
		public IBasicRepo<Employee> employees { get; private set; }
		public IBasicRepo<Order> orders { get; private set; }
		public IBasicRepo<Product> products { get; private set; }
		public IBasicRepo<Wishlist> wishlists { get; private set; }
        public IBasicRepo<Contact> contacts { get; private set; }
        public IBasicRepo<OrderProduct> orderProducts { get; private set; }
        public IUserInfoRepo userRepo { get; private set; }
        public IBasicRepo<Payment> payments { get; private set; }
		public ISearchRepo search { get; private set; }
		public async Task<int> CommitChangesAsync()
		{
			return await _db.SaveChangesAsync();
		}

		public void Dispose()
		{
			_db.Dispose();
		}
	}
}