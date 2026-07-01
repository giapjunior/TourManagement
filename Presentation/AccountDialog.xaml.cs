using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace Presentation
{
    public partial class AccountDialog : Window
    {
        private readonly AccountService _accountService;
        private Account? _editing;

        public AccountDialog(Account? account = null)
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            _accountService = new AccountService(new AccountRepository(context), new CustomerRepository(context));
            _editing = account;

            if (account != null)
            {
                txtTitle.Text = "Sửa Tài khoản";
                txtUsername.Text = account.Username;
                txtUsername.IsEnabled = false; // Không cho đổi username

                // Chọn đúng Role trong ComboBox bằng cách so sánh Content
                foreach (ComboBoxItem item in cboRole.Items)
                {
                    if (item.Content?.ToString() == account.Role)
                    {
                        cboRole.SelectedItem = item;
                        break;
                    }
                }

                chkIsActive.IsChecked = account.IsActive ?? true;
            }
            else
            {
                // Thêm mới: mặc định chọn Customer
                cboRole.SelectedIndex = 1;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_editing == null && string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Password không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cboRole.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Role.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var role = (cboRole.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Customer";

            try
            {
                if (_editing == null)
                {
                    // Thêm mới
                    _accountService.Add(new Account
                    {
                        Username = txtUsername.Text.Trim(),
                        Password = txtPassword.Password.Trim(),
                        Role = role,
                        IsActive = chkIsActive.IsChecked == true,
                        CreatedAt = DateTime.Now
                    });
                }
                else
                {
                    // Sửa: chỉ đổi password nếu user nhập vào
                    if (!string.IsNullOrWhiteSpace(txtPassword.Password))
                        _editing.Password = txtPassword.Password.Trim();
                    _editing.Role = role;
                    _editing.IsActive = chkIsActive.IsChecked == true;
                    _accountService.Update(_editing);
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
