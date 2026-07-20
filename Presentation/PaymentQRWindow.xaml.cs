using System.Windows;
using System.Threading.Tasks;

namespace Presentation
{
    public partial class PaymentQRWindow : Window
    {
        public bool IsPaymentConfirmed { get; private set; } = false;

        public PaymentQRWindow(decimal amount)
        {
            InitializeComponent();
            txtAmount.Text = $"Số tiền: {amount:N0} VNĐ";
        }

        private async void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            if (btn != null)
            {
                btn.Content = "⏳ Đang xác nhận giao dịch...";
                btn.IsEnabled = false;
            }
            
            // Giả lập thời gian chờ xác nhận từ ngân hàng/ví điện tử
            await Task.Delay(2000); 
            
            IsPaymentConfirmed = true;
            this.Close();
        }
    }
}
