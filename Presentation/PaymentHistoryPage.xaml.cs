using System.Windows.Controls;
using Service;
using Repository;
using DataAccess.DataAccess;

namespace Presentation
{
    public partial class PaymentHistoryPage : Page
    {
        private readonly IPaymentService _paymentService;

        public PaymentHistoryPage()
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            _paymentService = new PaymentService(new PaymentRepository(context));
            LoadData();
        }

        private void LoadData()
        {
            if (SessionManager.CurrentCustomer != null)
            {
                dgPayments.ItemsSource = _paymentService.GetByCustomerId(SessionManager.CurrentCustomer.CustomerId);
            }
        }
    }
}
