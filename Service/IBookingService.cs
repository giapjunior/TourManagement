using DataAccess.DataAccess;

namespace Service
{
    public interface IBookingService
    {
        List<Booking> GetAll();
        List<Booking> GetByCustomerId(int customerId);
        Booking GetById(int id);
        void Add(Booking booking);
        // Lọc booking theo trạng thái
        List<Booking> GetByStatus(string status);
        // Tìm kiếm booking theo tên khách hàng hoặc mã booking
        List<Booking> Search(string keyword);
        // Xác nhận booking (Pending → Confirmed)
        void ConfirmBooking(int bookingId);
        // Hủy booking kèm lý do, hoàn lại chỗ
        void CancelBooking(int bookingId, string cancelReason);
        // Đánh dấu hoàn thành
        void CompleteBooking(int bookingId);
        // Cập nhật trạng thái chung
        void UpdateStatus(int bookingId, string status);
    }
}
