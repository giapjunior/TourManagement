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
            LoadData();
        }

        private BookingService CreateBookingService()
        {
            var context = new TourManagementDbContext();
            return new BookingService(new BookingRepository(context), new ScheduleRepository(context));
        }

        private void LoadData()
        {
            var customerId = SessionManager.CurrentCustomer?.CustomerId ?? 0;
            dgMyBookings.ItemsSource = CreateBookingService().GetByCustomerId(customerId);
        }

        private void btnDetail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            var booking = CreateBookingService().GetById(id);
            if (booking != null)
            {
                new BookingDetailDialog(booking).ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            var svc = CreateBookingService();
            var booking = svc.GetById(id);
            
            if (booking == null) return;
            
            if (booking.Status != "Pending")
            {
                System.Windows.MessageBox.Show("Chỉ có thể hủy booking ở trạng thái Pending.", "Cảnh báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            var dialog = new CancelReasonDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    svc.CancelBooking(id, dialog.CancelReason);
                    System.Windows.MessageBox.Show("Hủy booking thành công.", "Thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    LoadData();
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private void btnPay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var booking = (DataAccess.DataAccess.Booking)((Button)sender).Tag;
            if (booking.Status != "Confirmed")
            {
                System.Windows.MessageBox.Show("Chỉ có thể thanh toán cho booking đã được Admin xác nhận (Confirmed).", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            var dialog = new PaymentDialog(booking.TotalPrice);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var context = new TourManagementDbContext();
                    var paymentService = new PaymentService(new PaymentRepository(context), new BookingRepository(context));
                    paymentService.ProcessPayment(booking.BookingId, dialog.SelectedMethod);
                    System.Windows.MessageBox.Show("Thanh toán thành công! Trạng thái đã được cập nhật.", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    LoadData();
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Lỗi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }
        private void btnReview_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var booking = (DataAccess.DataAccess.Booking)((Button)sender).Tag;
            if (booking.Status != "Completed")
            {
                System.Windows.MessageBox.Show("Chỉ có thể đánh giá những chuyến đi đã hoàn thành (Completed).", "Thông báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            var dialog = new ReviewDialog(booking.Schedule.TourId);
            dialog.ShowDialog();
        }
    }
}
