using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace Presentation
{
    public partial class StatisticsPage : Page
    {
        private readonly StatisticsService _statsService;

        public StatisticsPage()
        {
            // Khởi tạo service TRƯỚC InitializeComponent
            var context = new TourManagementDbContext();
            _statsService = new StatisticsService(
                new PaymentRepository(context),
                new BookingRepository(context),
                new CustomerRepository(context),
                new AccountRepository(context));
            InitializeComponent();
            LoadInitialData();
        }

        /// <summary>
        /// Load dữ liệu ban đầu cho tất cả các tab
        /// </summary>
        private void LoadInitialData()
        {
            // Load danh sách năm có dữ liệu
            var years = _statsService.GetAvailableYears();
            cboRevenueYear.ItemsSource = years;
            cboCustomerYear.ItemsSource = years;
            if (years.Count > 0)
            {
                cboRevenueYear.SelectedIndex = 0;
                cboCustomerYear.SelectedIndex = 0;
            }

            // Load top tour
            LoadTopTours();

            // Load booking stats (không lọc)
            LoadBookingStats();

            // Load top khách hàng chi tiêu
            LoadTopCustomers();
        }

        #region Tab 1: Doanh thu

        private void cboRevenueYear_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cboRevenueYear.SelectedItem == null) return;
            LoadRevenue((int)cboRevenueYear.SelectedItem);
        }

        /// <summary>
        /// Doanh thu theo tháng: gọi StatisticsService, hiển thị bảng số liệu
        /// </summary>
        private void LoadRevenue(int year)
        {
            var data = _statsService.GetRevenueByMonth(year);
            // Chuyển Dictionary thành danh sách để bind vào DataGrid
            var list = data
                .OrderBy(d => d.Key)
                .Select(d => new { Month = $"Tháng {d.Key}", Revenue = d.Value })
                .ToList();
            dgRevenue.ItemsSource = list;

            // Hiển thị tổng doanh thu năm
            var total = data.Values.Sum();
            txtTotalRevenue.Text = $"Tổng năm {year}: {total:N0} VNĐ";
        }

        #endregion

        #region Tab 2: Booking theo trạng thái

        private void btnLoadBookingStats_Click(object sender, RoutedEventArgs e) => LoadBookingStats();

        /// <summary>
        /// Đếm booking theo trạng thái: dùng LINQ GroupBy
        /// </summary>
        private void LoadBookingStats()
        {
            DateTime? from = dpBookingFrom.SelectedDate;
            DateTime? to = dpBookingTo.SelectedDate;

            var data = _statsService.GetBookingCountByStatus(from, to);
            var list = data.Select(d => new { Status = d.Key, Count = d.Value }).ToList();
            dgBookingStats.ItemsSource = list;
        }

        #endregion

        #region Tab 3: Top Tour

        /// <summary>
        /// Top 10 tour được đặt nhiều nhất: dùng LINQ GroupBy + OrderByDescending + Take
        /// </summary>
        private void LoadTopTours()
        {
            var data = _statsService.GetTopBookedTours(10);
            var list = data.Select((d, index) => new
            {
                Rank = index + 1,
                TourName = d.TourName,
                BookingCount = d.BookingCount
            }).ToList();
            dgTopTours.ItemsSource = list;
        }

        #endregion

        #region Tab 4: Khách hàng

        private void cboCustomerYear_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cboCustomerYear.SelectedItem == null) return;
            LoadNewCustomers((int)cboCustomerYear.SelectedItem);
        }

        /// <summary>
        /// Khách hàng mới theo tháng
        /// </summary>
        private void LoadNewCustomers(int year)
        {
            var data = _statsService.GetNewCustomersByMonth(year);
            var list = data
                .OrderBy(d => d.Key)
                .Select(d => new { Month = $"Tháng {d.Key}", Count = d.Value })
                .ToList();
            dgNewCustomers.ItemsSource = list;
        }

        /// <summary>
        /// Top 10 khách hàng chi tiêu nhiều nhất
        /// </summary>
        private void LoadTopCustomers()
        {
            var data = _statsService.GetTopSpendingCustomers(10);
            var list = data.Select((d, index) => new
            {
                Rank = index + 1,
                CustomerName = d.CustomerName,
                TotalSpent = d.TotalSpent
            }).ToList();
            dgTopCustomers.ItemsSource = list;
        }

        #endregion
    }
}
