using DataAccess.DataAccess;
using Repository;

namespace Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IScheduleRepository _scheduleRepo;

        public BookingService(IBookingRepository bookingRepo, IScheduleRepository scheduleRepo)
        {
            _bookingRepo = bookingRepo;
            _scheduleRepo = scheduleRepo;
        }

        public List<Booking> GetAll() => _bookingRepo.GetAllWithDetails();

        public List<Booking> GetByCustomerId(int customerId) => _bookingRepo.GetByCustomerId(customerId);

        public Booking GetById(int id) => _bookingRepo.GetWithDetails(id);

        // Lọc booking theo trạng thái (dùng LINQ Where)
        public List<Booking> GetByStatus(string status) => _bookingRepo.GetByStatus(status);

        // Tìm kiếm theo tên khách hàng hoặc mã booking
        public List<Booking> Search(string keyword) => _bookingRepo.SearchWithDetails(keyword);

        public void Add(Booking booking)
        {
            if (booking.NumberOfPeople <= 0)
                throw new Exception("Số người phải lớn hơn 0.");

            var schedule = _scheduleRepo.GetById(booking.ScheduleId);
            if (schedule == null)
                throw new Exception("Lịch trình không tồn tại.");
            if (schedule.AvailableSlot < booking.NumberOfPeople)
                throw new Exception($"Chỉ còn {schedule.AvailableSlot} chỗ trống.");

            // Tính tổng tiền = giá tour * số người
            booking.TotalPrice = schedule.Tour.Price * booking.NumberOfPeople;
            booking.BookingDate = DateTime.Today;
            booking.Status = "Pending";

            // Trừ số chỗ trống
            schedule.AvailableSlot -= booking.NumberOfPeople;
            _scheduleRepo.Update(schedule);

            _bookingRepo.Add(booking);
        }

        /// <summary>
        /// Xác nhận booking: Pending → Confirmed
        /// Kiểm tra Schedule còn đủ chỗ (AvailableSlot >= NumberOfPeople nếu slot chưa bị trừ)
        /// </summary>
        public void ConfirmBooking(int bookingId)
        {
            var booking = _bookingRepo.GetWithDetails(bookingId);
            if (booking == null) throw new Exception("Booking không tồn tại.");
            if (booking.Status != "Pending")
                throw new Exception("Chỉ có thể xác nhận booking đang ở trạng thái Pending.");

            booking.Status = "Confirmed";
            _bookingRepo.Update(booking);
        }

        /// <summary>
        /// Hủy booking:
        /// - Đổi Status → Cancelled
        /// - Ghi lý do hủy (CancelReason)
        /// - Hoàn trả chỗ (AvailableSlot += NumberOfPeople)
        /// </summary>
        public void CancelBooking(int bookingId, string cancelReason)
        {
            var booking = _bookingRepo.GetWithDetails(bookingId);
            if (booking == null) throw new Exception("Booking không tồn tại.");
            if (booking.Status == "Cancelled")
                throw new Exception("Booking này đã bị hủy trước đó.");
            if (string.IsNullOrWhiteSpace(cancelReason))
                throw new Exception("Vui lòng nhập lý do hủy booking.");

            // Hoàn trả chỗ vào Schedule tương ứng
            var schedule = _scheduleRepo.GetById(booking.ScheduleId);
            if (schedule != null)
            {
                schedule.AvailableSlot += booking.NumberOfPeople;
                _scheduleRepo.Update(schedule);
            }

            booking.Status = "Cancelled";
            booking.CancelReason = cancelReason;
            _bookingRepo.Update(booking);
        }

        /// <summary>
        /// Đánh dấu booking hoàn thành sau khi tour kết thúc
        /// </summary>
        public void CompleteBooking(int bookingId)
        {
            var booking = _bookingRepo.GetById(bookingId);
            if (booking == null) throw new Exception("Booking không tồn tại.");
            if (booking.Status != "Confirmed")
                throw new Exception("Chỉ có thể đánh dấu hoàn thành booking đã được xác nhận.");

            booking.Status = "Completed";
            _bookingRepo.Update(booking);
        }

        public void UpdateStatus(int bookingId, string status)
        {
            var booking = _bookingRepo.GetById(bookingId);
            if (booking == null) throw new Exception("Booking không tồn tại.");
            booking.Status = status;
            _bookingRepo.Update(booking);
        }

        public void Delete(int id) => _bookingRepo.Delete(id);
    }
}
