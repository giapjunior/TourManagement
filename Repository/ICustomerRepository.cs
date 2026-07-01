using DataAccess.DataAccess;

namespace Repository
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Customer? GetByAccountId(int accountId);
        // Lấy danh sách khách hàng kèm thông tin Account (để hiển thị trạng thái tài khoản)
        List<Customer> GetAllWithAccount();
        // Tìm kiếm khách hàng theo tên, email hoặc số điện thoại
        List<Customer> Search(string keyword);
        // Lấy khách hàng kèm Account theo CustomerId
        Customer? GetByIdWithAccount(int customerId);
    }
}
