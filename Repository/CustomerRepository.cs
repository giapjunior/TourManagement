using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TourManagementDbContext _context;

        public CustomerRepository(TourManagementDbContext context)
        {
            _context = context;
        }

        public List<Customer> GetAll() => _context.Customers.ToList();

        public Customer GetById(int id) => _context.Customers.FirstOrDefault(c => c.CustomerId == id);

        public void Add(Customer entity)
        {
            _context.Customers.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Customer entity)
        {
            _context.Customers.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Customers.Remove(entity);
                _context.SaveChanges();
            }
        }

        public Customer? GetByAccountId(int accountId)
            => _context.Customers.FirstOrDefault(c => c.AccountId == accountId);

        // Include Account để lấy thông tin IsActive cho Admin
        public List<Customer> GetAllWithAccount()
            => _context.Customers.Include(c => c.Account).ToList();

        // Tìm kiếm theo FullName, Email, Phone bằng LINQ Where + Contains
        public List<Customer> Search(string keyword)
            => _context.Customers
                .Include(c => c.Account)
                .Where(c => c.FullName.Contains(keyword)
                         || c.Email.Contains(keyword)
                         || c.Phone.Contains(keyword))
                .ToList();

        // Lấy khách hàng kèm Account theo ID
        public Customer? GetByIdWithAccount(int customerId)
            => _context.Customers
                .Include(c => c.Account)
                .FirstOrDefault(c => c.CustomerId == customerId);
    }
}
