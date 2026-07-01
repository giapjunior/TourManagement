using DataAccess.DataAccess;
using Repository;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ICustomerRepository _customerRepo;

        public AccountService(IAccountRepository accountRepo, ICustomerRepository customerRepo)
        {
            _accountRepo = accountRepo;
            _customerRepo = customerRepo;
        }

        public Account? Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;
            return _accountRepo.Login(username, password);
        }

        /// <summary>
        /// Lấy account theo username (dùng cho kiểm tra đăng nhập riêng biệt: tồn tại? bị khóa?)
        /// </summary>
        public Account? GetByUsername(string username) => _accountRepo.GetByUsername(username);

        public List<Account> GetAll() => _accountRepo.GetAll();

        public Account GetById(int id) => _accountRepo.GetById(id);

        public void Add(Account account)
        {
            if (string.IsNullOrWhiteSpace(account.Username))
                throw new Exception("Username không được để trống.");
            if (_accountRepo.GetByUsername(account.Username) != null)
                throw new Exception("Username đã tồn tại.");
            if (string.IsNullOrWhiteSpace(account.Password))
                throw new Exception("Password không được để trống.");
            _accountRepo.Add(account);
        }

        public void Update(Account account)
        {
            if (string.IsNullOrWhiteSpace(account.Username))
                throw new Exception("Username không được để trống.");
            if (string.IsNullOrWhiteSpace(account.Password))
                throw new Exception("Password không được để trống.");
            _accountRepo.Update(account);
        }

        public void Delete(int id) => _accountRepo.Delete(id);

        /// <summary>
        /// Đăng ký tài khoản Customer mới:
        /// - Validate: username trùng, password tối thiểu 6 ký tự, email format, phone format
        /// - Insert Account + Customer trong 1 transaction (nếu 1 lỗi thì rollback cả 2)
        /// </summary>
        public void Register(string username, string password, string fullName, string phone, string email, string? address)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("Username không được để trống.");
            if (_accountRepo.GetByUsername(username) != null)
                throw new Exception("Username đã tồn tại trong hệ thống.");
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new Exception("Mật khẩu phải có tối thiểu 6 ký tự.");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new Exception("Họ tên không được để trống.");
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email không được để trống.");
            if (string.IsNullOrWhiteSpace(phone))
                throw new Exception("Số điện thoại không được để trống.");

            // Validate email format bằng MailAddress
            try { var addr = new System.Net.Mail.MailAddress(email); }
            catch { throw new Exception("Email không đúng định dạng."); }

            // Validate phone: chỉ chứa số, 10 ký tự
            if (!phone.All(char.IsDigit) || phone.Length != 10)
                throw new Exception("Số điện thoại phải gồm đúng 10 chữ số.");

            // Tạo Account + Customer trong 1 transaction
            // Lấy DbContext từ repository để dùng chung transaction
            var account = new Account
            {
                Username = username,
                Password = password,
                Role = "Customer",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            // Insert Account trước (để lấy AccountId)
            _accountRepo.Add(account);

            try
            {
                // Insert Customer gắn AccountId vừa tạo
                var customer = new Customer
                {
                    AccountId = account.AccountId,
                    FullName = fullName,
                    Phone = phone,
                    Email = email,
                    Address = address
                };
                _customerRepo.Add(customer);
            }
            catch (Exception)
            {
                // Nếu insert Customer lỗi → xóa Account vừa tạo (rollback thủ công)
                _accountRepo.Delete(account.AccountId);
                throw new Exception("Đăng ký thất bại. Vui lòng thử lại.");
            }
        }
    }
}
