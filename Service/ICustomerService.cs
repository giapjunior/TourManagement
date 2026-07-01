using DataAccess.DataAccess;

namespace Service
{
    public interface ICustomerService
    {
        List<Customer> GetAll();
        Customer GetById(int id);
        Customer? GetByAccountId(int accountId);
        void Add(Customer customer);
        void Update(Customer customer);
        // Lấy danh sách kèm thông tin Account
        List<Customer> GetAllWithAccount();
        // Tìm kiếm khách hàng
        List<Customer> Search(string keyword);
        // Khóa/mở khóa tài khoản khách hàng
        void ToggleAccountStatus(int customerId);
    }
}
