using DataAccess.DataAccess;

namespace Repository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Payment? GetByBookingId(int bookingId);
        // Lấy tất cả Payment kèm thông tin Booking, Schedule, Tour cho thống kê
        List<Payment> GetAllWithDetails();
    }
}
