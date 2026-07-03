using System.Windows;

namespace Presentation
{
    public partial class PaymentDialog : Window
    {
        public string SelectedMethod { get; private set; }

        public PaymentDialog(decimal totalAmount)
        {
            InitializeComponent();
            txtTotal.Text = $"{totalAmount:N0} VNĐ";
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedMethod = cboMethod.Text;
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
