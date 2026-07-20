using System.Windows;

namespace Presentation
{
    public partial class PaymentDialog : Window
    {
        public string SelectedMethod { get; private set; } = "Card";
        private decimal _totalAmount;

        public PaymentDialog(decimal totalAmount)
        {
            InitializeComponent();
            _totalAmount = totalAmount;
            txtTotal.Text = $"{totalAmount:N0} VNĐ";
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedMethod = cboMethod.Text;
            
            if (SelectedMethod == "Transfer")
            {
                var qrWindow = new PaymentQRWindow(_totalAmount);
                qrWindow.Owner = this;
                qrWindow.ShowDialog();
                
                if (!qrWindow.IsPaymentConfirmed)
                {
                    // User closed the QR window without paying
                    return;
                }
            }
            
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
