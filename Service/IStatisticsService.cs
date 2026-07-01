namespace Service
{
    /// <summary>
    /// Interface cho các truy vấn thống kê - báo cáo.
    /// Tất cả logic tính toán nằm trong Service, Presentation chỉ gọi và hiển thị.
    /// </summary>
    public interface IStatisticsService
    {
        // Doanh thu theo tháng trong 1 năm (trả key=tháng, value=tổng tiền)
        Dictionary<int, decimal> GetRevenueByMonth(int year);
        // Doanh thu theo khoảng thời gian
        decimal GetRevenueByDateRange(DateTime from, DateTime to);
        // Đếm booking theo trạng thái trong khoảng thời gian
        Dictionary<string, int> GetBookingCountByStatus(DateTime? from, DateTime? to);
        // Top tour được đặt nhiều nhất
        List<(string TourName, int BookingCount)> GetTopBookedTours(int top);
        // Số khách hàng mới theo tháng trong 1 năm
        Dictionary<int, int> GetNewCustomersByMonth(int year);
        // Top khách hàng chi tiêu nhiều nhất
        List<(string CustomerName, decimal TotalSpent)> GetTopSpendingCustomers(int top);
        // Lấy danh sách các năm có dữ liệu Payment
        List<int> GetAvailableYears();
    }
}
