using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace Presentation
{
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// Tạo service MỚI cho mỗi operation để tránh EF tracking conflict
        /// </summary>
        private AccountService CreateService()
        {
            var context = new TourManagementDbContext();
            return new AccountService(new AccountRepository(context), new CustomerRepository(context));
        }

        private void LoadData()
        {
            // Load danh sách tất cả tài khoản
            dgAccount.ItemsSource = CreateService().GetAll();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AccountDialog();
            if (dialog.ShowDialog() == true) LoadData();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            var account = CreateService().GetById(id);
            if (account == null) return;
            var dialog = new AccountDialog(account);
            if (dialog.ShowDialog() == true) LoadData();
        }

        /// <summary>
        /// Xóa tài khoản:
        /// - Không được xóa tài khoản đang đăng nhập
        /// - Xóa cứng khỏi DB → biến mất khỏi danh sách ngay
        /// - Nếu có Customer FK → hiển thị lỗi rõ ràng
        /// </summary>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;

            if (id == SessionManager.CurrentAccount?.AccountId)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?\nHành động này không thể hoàn tác.",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    CreateService().Delete(id);
                    LoadData(); // Reload ngay → tài khoản biến mất khỏi danh sách
                    MessageBox.Show("Đã xóa tài khoản thành công.", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Nếu FK constraint (Account có Customer liên quan)
                    MessageBox.Show("Không thể xóa: " + ex.Message,
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
