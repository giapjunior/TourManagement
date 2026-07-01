using DataAccess.DataAccess;

namespace Repository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        List<Booking> GetByCustomerId(int customerId);
        List<Booking> GetAllWithDetails();
        Booking GetWithDetails(int bookingId);
        // Lọc booking theo trạng thái
        List<Booking> GetByStatus(string status);
        // Tìm kiếm booking theo tên khách hàng hoặc mã booking
        List<Booking> SearchWithDetails(string keyword);
    }
}
