using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TourManagementDbContext _context;

        public AccountRepository(TourManagementDbContext context)
        {
            _context = context;
        }

        public List<Account> GetAll() => _context.Accounts.ToList();

        public Account GetById(int id) => _context.Accounts.FirstOrDefault(a => a.AccountId == id);

        public void Add(Account entity)
        {
            _context.Accounts.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Account entity)
        {
            _context.Accounts.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Accounts.Remove(entity);
                _context.SaveChanges();
            }
        }

        public Account? Login(string username, string password)
            => _context.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password && a.IsActive == true);

        public Account? GetByUsername(string username)
            => _context.Accounts.FirstOrDefault(a => a.Username == username);
    }
}
