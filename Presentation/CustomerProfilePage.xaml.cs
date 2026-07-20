using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace Presentation
{
    public partial class CustomerProfilePage : Page
    {
        private readonly CustomerService _customerService;
        private readonly AccountService _accountService;

        public CustomerProfilePage()
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            _customerService = new CustomerService(new CustomerRepository(context), new AccountRepository(context));
            _accountService = new AccountService(new AccountRepository(context), new CustomerRepository(context));

            LoadProfileData();
        }

        private void LoadProfileData()
        {
            var customer = SessionManager.CurrentCustomer;
            if (customer != null)
            {
                txtFullName.Text = customer.FullName;
                txtPhone.Text = customer.Phone;
                txtEmail.Text = customer.Email;
                txtAddress.Text = customer.Address;
            }
        }

        private void btnToggleEdit_Click(object sender, RoutedEventArgs e)
        {
            bool isEditing = !pnlForm.IsEnabled;
            pnlForm.IsEnabled = isEditing;
            
            if (isEditing)
            {
                btnToggleEdit.Content = "❌ Hủy";
                btnToggleEdit.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#EF4444"));
                btnSave.Visibility = Visibility.Visible;
            }
            else
            {
                btnToggleEdit.Content = "✏️ Sửa thông tin";
                btnToggleEdit.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2563EB"));
                btnSave.Visibility = Visibility.Collapsed;
                // Reload original data to discard changes
                LoadProfileData();
                txtPassword.Password = "";
                txtConfirmPassword.Password = "";
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customer = SessionManager.CurrentCustomer;
                var account = SessionManager.CurrentAccount;

                if (customer == null || account == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin phiên đăng nhập. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Update Customer Info
                customer.FullName = txtFullName.Text.Trim();
                customer.Phone = txtPhone.Text.Trim();
                customer.Email = txtEmail.Text.Trim();
                customer.Address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
                
                // Phone & Email Validation could be added here
                if (string.IsNullOrWhiteSpace(customer.FullName) || string.IsNullOrWhiteSpace(customer.Phone) || string.IsNullOrWhiteSpace(customer.Email))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ các trường bắt buộc (*).", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!customer.Phone.All(char.IsDigit) || customer.Phone.Length != 10)
                {
                    MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _customerService.Update(customer);

                // Update Password if provided
                string newPassword = txtPassword.Password;
                string confirmPassword = txtConfirmPassword.Password;

                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (newPassword.Length < 6)
                    {
                        MessageBox.Show("Mật khẩu mới phải có tối thiểu 6 ký tự.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (newPassword != confirmPassword)
                    {
                        MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    account.Password = newPassword;
                    _accountService.Update(account);
                }

                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Exit edit mode after saving
                pnlForm.IsEnabled = false;
                btnToggleEdit.Content = "✏️ Sửa thông tin";
                btnToggleEdit.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2563EB"));
                btnSave.Visibility = Visibility.Collapsed;
                txtPassword.Password = "";
                txtConfirmPassword.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
