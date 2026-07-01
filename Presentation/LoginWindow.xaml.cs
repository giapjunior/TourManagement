using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;

namespace Presentation
{
    public partial class LoginWindow : Window
    {
        private readonly AccountService _accountService;
        private readonly CustomerService _customerService;

        public LoginWindow()
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            _accountService = new AccountService(new AccountRepository(context), new CustomerRepository(context));
            _customerService = new CustomerService(new CustomerRepository(context), new AccountRepository(context));
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password.Trim();

            // Validate không để trống
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Vui lòng nhập tên đăng nhập và mật khẩu.");
                return;
            }

            // Kiểm tra tài khoản tồn tại (tách riêng để phân biệt lỗi)
            var account = _accountService.GetByUsername(username);

            if (account == null || account.Password != password)
            {
                // Sai tên đăng nhập hoặc mật khẩu
                ShowError("Sai tên đăng nhập hoặc mật khẩu.");
                return;
            }

            // Kiểm tra tài khoản bị khóa
            if (account.IsActive == false)
            {
                ShowError("Tài khoản đã bị khóa, vui lòng liên hệ quản trị viên.");
                return;
            }

            // Đăng nhập thành công → lưu session
            SessionManager.CurrentAccount = account;

            if (account.Role == "Customer")
            {
                SessionManager.CurrentCustomer = _customerService.GetByAccountId(account.AccountId);
                new CustomerWindow().Show();
            }
            else if (account.Role == "Admin")
            {
                new AdminWindow().Show();
            }

            this.Close();
        }

        /// <summary>
        /// Mở màn hình Đăng ký tài khoản Customer
        /// </summary>
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        /// <summary>
        /// Hiển thị lỗi trên form
        /// </summary>
        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
