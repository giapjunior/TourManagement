using DataAccess.DataAccess;
using System.Windows;

namespace Presentation
{
    public partial class CustomerDetailDialog : Window
    {
        public CustomerDetailDialog(Customer customer, List<Booking> bookings)
        {
            InitializeComponent();
            LoadCustomerDetails(customer, bookings);
        }

        /// <summary>
        /// Hiển thị thông tin khách hàng và lịch sử booking (join Booking)
        /// </summary>
        private void LoadCustomerDetails(Customer customer, List<Booking> bookings)
        {
            txtName.Text = $"Họ tên: {customer.FullName}";
            txtPhone.Text = $"Điện thoại: {customer.Phone}";
            txtEmail.Text = $"Email: {customer.Email}";
            txtAddress.Text = $"Địa chỉ: {customer.Address ?? "N/A"}";

            // Hiển thị trạng thái tài khoản
            var isActive = customer.Account?.IsActive ?? true;
            txtAccountStatus.Text = isActive ? "Trạng thái TK: ✅ Hoạt động" : "Trạng thái TK: ❌ Đã khóa";
            txtAccountStatus.Foreground = isActive
                ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(16, 185, 129))
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(239, 68, 68));

            // Hiển thị lịch sử booking trong DataGrid
            dgBookings.ItemsSource = bookings;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();
    }
}
