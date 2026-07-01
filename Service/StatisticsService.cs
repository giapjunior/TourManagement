using DataAccess.DataAccess;
using Repository;

namespace Service
{
    /// <summary>
    /// Service thống kê - báo cáo.
    /// Tất cả logic tính toán dùng LINQ (GroupBy, OrderByDescending, Sum, Count, Take).
    /// Presentation chỉ gọi Service và hiển thị kết quả.
    /// </summary>
    public class StatisticsService : IStatisticsService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IAccountRepository _accountRepo;

        public StatisticsService(
            IPaymentRepository paymentRepo,
            IBookingRepository bookingRepo,
            ICustomerRepository customerRepo,
            IAccountRepository accountRepo)
        {
            _paymentRepo = paymentRepo;
            _bookingRepo = bookingRepo;
            _customerRepo = customerRepo;
            _accountRepo = accountRepo;
        }

        /// <summary>
        /// Doanh thu theo tháng: GroupBy tháng của PaymentDate, Sum Amount
        /// </summary>
        public Dictionary<int, decimal> GetRevenueByMonth(int year)
        {
            var payments = _paymentRepo.GetAll();
            // Dùng LINQ GroupBy tháng, Sum Amount
            var result = payments
                .Where(p => p.PaymentDate.HasValue && p.PaymentDate.Value.Year == year)
                .GroupBy(p => p.PaymentDate!.Value.Month)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));

            // Đảm bảo trả 12 tháng (tháng không có doanh thu = 0)
            for (int i = 1; i <= 12; i++)
                if (!result.ContainsKey(i)) result[i] = 0;

            return result;
        }

        /// <summary>
        /// Doanh thu theo khoảng thời gian
        /// </summary>
        public decimal GetRevenueByDateRange(DateTime from, DateTime to)
        {
            var payments = _paymentRepo.GetAll();
            return payments
                .Where(p => p.PaymentDate.HasValue
                         && p.PaymentDate.Value >= from
                         && p.PaymentDate.Value <= to)
                .Sum(p => p.Amount);
        }

        /// <summary>
        /// Đếm booking theo trạng thái: GroupBy Status, Count
        /// </summary>
        public Dictionary<string, int> GetBookingCountByStatus(DateTime? from, DateTime? to)
        {
            var bookings = _bookingRepo.GetAllWithDetails();

            // Lọc theo khoảng thời gian nếu có
            if (from.HasValue)
                bookings = bookings.Where(b => b.BookingDate >= from.Value).ToList();
            if (to.HasValue)
                bookings = bookings.Where(b => b.BookingDate <= to.Value).ToList();

            // Dùng LINQ GroupBy Status
            return bookings
                .GroupBy(b => b.Status)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Top tour được đặt nhiều nhất: GroupBy Tour, OrderByDescending Count, Take
        /// </summary>
        public List<(string TourName, int BookingCount)> GetTopBookedTours(int top)
        {
            var bookings = _bookingRepo.GetAllWithDetails();
            return bookings
                .GroupBy(b => b.Schedule.Tour.TourName)
                .OrderByDescending(g => g.Count())
                .Take(top)
                .Select(g => (g.Key, g.Count()))
                .ToList();
        }

        /// <summary>
        /// Số khách hàng mới theo tháng: GroupBy CreatedAt month
        /// </summary>
        public Dictionary<int, int> GetNewCustomersByMonth(int year)
        {
            var accounts = _accountRepo.GetAll();
            var result = accounts
                .Where(a => a.Role == "Customer" && a.CreatedAt.HasValue && a.CreatedAt.Value.Year == year)
                .GroupBy(a => a.CreatedAt!.Value.Month)
                .ToDictionary(g => g.Key, g => g.Count());

            // Đảm bảo trả 12 tháng
            for (int i = 1; i <= 12; i++)
                if (!result.ContainsKey(i)) result[i] = 0;

            return result;
        }

        /// <summary>
        /// Top khách hàng chi tiêu nhiều nhất: GroupBy Customer, Sum Payment Amount
        /// </summary>
        public List<(string CustomerName, decimal TotalSpent)> GetTopSpendingCustomers(int top)
        {
            var payments = _paymentRepo.GetAllWithDetails();
            return payments
                .Where(p => p.Booking?.Customer != null)
                .GroupBy(p => p.Booking.Customer.FullName)
                .OrderByDescending(g => g.Sum(p => p.Amount))
                .Take(top)
                .Select(g => (g.Key, g.Sum(p => p.Amount)))
                .ToList();
        }

        /// <summary>
        /// Lấy danh sách các năm có dữ liệu Payment (để cho user chọn năm)
        /// </summary>
        public List<int> GetAvailableYears()
        {
            var payments = _paymentRepo.GetAll();
            var years = payments
                .Where(p => p.PaymentDate.HasValue)
                .Select(p => p.PaymentDate!.Value.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            // Luôn đảm bảo có năm hiện tại
            if (!years.Contains(DateTime.Now.Year))
                years.Insert(0, DateTime.Now.Year);

            return years;
        }
    }
}
