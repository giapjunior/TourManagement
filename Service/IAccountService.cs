using DataAccess.DataAccess;

namespace Service
{
    public interface IAccountService
    {
        Account? Login(string username, string password);
        Account? GetByUsername(string username);
        List<Account> GetAll();
        Account GetById(int id);
        void Add(Account account);
        void Update(Account account);
        void Delete(int id);
        /// <summary>
        /// Đăng ký tài khoản Customer: tạo Account + Customer trong 1 transaction
        /// </summary>
        void Register(string username, string password, string fullName, string phone, string email, string? address);
    }
}
