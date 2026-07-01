using DataAccess.DataAccess;

namespace Repository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Account? Login(string username, string password);
        Account? GetByUsername(string username);
    }
}
