using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation
{
    public partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Tạo mới DbContext + Service cho mỗi operation để tránh EF tracking bug
        /// (Entity Framework: dùng chung DbContext → change tracker bẩn → update lây sang entity khác)
        /// </summary>
        private CustomerService CreateService()
        {
            var context = new TourManagementDbContext();
            return new CustomerService(new CustomerRepository(context), new AccountRepository(context));
        }

        private BookingService CreateBookingService()
        {
            var context = new TourManagementDbContext();
            return new BookingService(new BookingRepository(context), new ScheduleRepository(context));
        }

        private void LoadData(string keyword = "")
        {
            var svc = CreateService();
            dgCustomer.ItemsSource = string.IsNullOrWhiteSpace(keyword)
                ? svc.GetAllWithAccount()
                : svc.Search(keyword);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) => LoadData(txtSearch.Text);
        private void txtSearch_KeyUp(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LoadData(txtSearch.Text); }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            var svc = CreateService();
            var customer = svc.GetAllWithAccount().FirstOrDefault(c => c.CustomerId == id);
            if (customer != null)
            {
                var bookings = CreateBookingService().GetByCustomerId(id);
                new CustomerDetailDialog(customer, bookings).ShowDialog();
            }
        }

        /// <summary>
        /// Khóa/mở khóa tài khoản:
        /// Dùng DbContext MỚI cho mỗi lần toggle để tránh EF tracking conflict
        /// </summary>
        private void btnToggle_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;

            // Lấy thông tin trước để hiển thị trạng thái hiện tại cho user
            var current = CreateService().GetAllWithAccount().FirstOrDefault(c => c.CustomerId == id);
            var currentStatus = current?.Account?.IsActive == true ? "Hoạt động" : "Đã khóa";
            var action = current?.Account?.IsActive == true ? "khóa" : "mở khóa";

            if (MessageBox.Show($"Bạn có chắc muốn {action} tài khoản \"{current?.FullName}\"?\nTrạng thái hiện tại: {currentStatus}",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Tạo service MỚI để đảm bảo DbContext sạch
                    CreateService().ToggleAccountStatus(id);
                    MessageBox.Show($"Đã {action} tài khoản thành công.", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
