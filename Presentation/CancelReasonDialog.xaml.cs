using System.Windows;

namespace Presentation
{
    public partial class CancelReasonDialog : Window
    {
        /// <summary>
        /// Lý do hủy booking do Admin nhập
        /// </summary>
        public string CancelReason { get; private set; } = "";

        public CancelReasonDialog()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("Vui lòng nhập lý do hủy.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CancelReason = txtReason.Text.Trim();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
