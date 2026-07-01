using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows.Controls;

namespace Presentation
{
    public partial class MyBookingsPage : Page
    {
        public MyBookingsPage()
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            var bookingService = new BookingService(new BookingRepository(context), new ScheduleRepository(context));
            var customerId = SessionManager.CurrentCustomer?.CustomerId ?? 0;
            dgMyBookings.ItemsSource = bookingService.GetByCustomerId(customerId);
        }
    }
}
