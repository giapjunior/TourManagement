using DataAccess.DataAccess;
using Repository;

namespace Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IAccountRepository _accountRepo;

        public CustomerService(ICustomerRepository customerRepo, IAccountRepository accountRepo = null)
        {
            _customerRepo = customerRepo;
            _accountRepo = accountRepo;
        }

        public List<Customer> GetAll() => _customerRepo.GetAll();

        public Customer GetById(int id) => _customerRepo.GetById(id);

        public Customer? GetByAccountId(int accountId) => _customerRepo.GetByAccountId(accountId);

        // Lấy danh sách kèm Account (Admin dùng để xem trạng thái tài khoản)
        public List<Customer> GetAllWithAccount() => _customerRepo.GetAllWithAccount();

        // Tìm kiếm theo tên, email, SDT bằng LINQ
        public List<Customer> Search(string keyword) => _customerRepo.Search(keyword);

        public void Add(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FullName))
                throw new Exception("Họ tên không được để trống.");
            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new Exception("Email không được để trống.");
            _customerRepo.Add(customer);
        }

        public void Update(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FullName))
                throw new Exception("Họ tên không được để trống.");
            _customerRepo.Update(customer);
        }

        /// <summary>
        /// Khóa/mở khóa tài khoản khách hàng:
        /// - Lấy Customer kèm Account
        /// - Đổi Account.IsActive (toggle true ↔ false)
        /// </summary>
        public void ToggleAccountStatus(int customerId)
        {
            var customer = _customerRepo.GetByIdWithAccount(customerId);
            if (customer == null)
                throw new Exception("Khách hàng không tồn tại.");
            if (customer.Account == null)
                throw new Exception("Tài khoản không tồn tại.");

            // Toggle trạng thái: true → false, false → true
            customer.Account.IsActive = !(customer.Account.IsActive ?? true);
            _accountRepo.Update(customer.Account);
        }

        public void Delete(int id) => _customerRepo.Delete(id);
    }
}
