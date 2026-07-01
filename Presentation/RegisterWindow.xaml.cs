using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;

namespace Presentation
{
    public partial class RegisterWindow : Window
    {
        private readonly AccountService _accountService;

        public RegisterWindow()
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            _accountService = new AccountService(new AccountRepository(context), new CustomerRepository(context));
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;

            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password;
            var confirmPassword = txtConfirmPassword.Password;
            var fullName = txtFullName.Text.Trim();
            var phone = txtPhone.Text.Trim();
            var email = txtEmail.Text.Trim();
            var address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();

            // Validate ở UI trước khi gọi Service
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)
                || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone)
                || string.IsNullOrWhiteSpace(email))
            {
                ShowError("Vui lòng điền đầy đủ các trường bắt buộc (*).");
                return;
            }

            // Password và Xác nhận Password phải khớp
            if (password != confirmPassword)
            {
                ShowError("Mật khẩu và Xác nhận mật khẩu không khớp.");
                return;
            }

            try
            {
                // Gọi AccountService.Register — tạo Account + Customer trong 1 transaction
                _accountService.Register(username, password, fullName, phone, email, address);

                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
