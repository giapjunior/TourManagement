using DataAccess.DataAccess;
using System.Windows;

namespace Presentation
{
    public partial class BookingDetailDialog : Window
    {
        public BookingDetailDialog(Booking booking)
        {
            InitializeComponent();
            LoadBookingDetails(booking);
        }

        /// <summary>
        /// Hiển thị đầy đủ thông tin booking: khách hàng, tour, lịch trình, thanh toán
        /// </summary>
        private void LoadBookingDetails(Booking booking)
        {
            // Thông tin Booking
            txtBookingId.Text = $"Mã booking: #{booking.BookingId}";
            txtBookingDate.Text = $"Ngày đặt: {booking.BookingDate:dd/MM/yyyy}";
            txtNumPeople.Text = $"Số người: {booking.NumberOfPeople}";
            txtTotalPrice.Text = $"Tổng tiền: {booking.TotalPrice:N0} VNĐ";
            txtStatus.Text = $"Trạng thái: {booking.Status}";

            // Hiển thị lý do hủy nếu có
            if (!string.IsNullOrEmpty(booking.CancelReason))
            {
                txtCancelReason.Text = $"Lý do hủy: {booking.CancelReason}";
                txtCancelReason.Visibility = Visibility.Visible;
            }

            // Thông tin Khách hàng
            if (booking.Customer != null)
            {
                txtCustName.Text = $"Họ tên: {booking.Customer.FullName}";
                txtCustPhone.Text = $"Điện thoại: {booking.Customer.Phone}";
                txtCustEmail.Text = $"Email: {booking.Customer.Email}";
            }

            // Thông tin Tour + Lịch trình
            if (booking.Schedule != null)
            {
                txtTourName.Text = $"Tour: {booking.Schedule.Tour?.TourName}";
                txtDestination.Text = $"Điểm đến: {booking.Schedule.Tour?.Destination}";
                txtDeparture.Text = $"Ngày khởi hành: {booking.Schedule.DepartureDate}";
                txtReturn.Text = $"Ngày về: {booking.Schedule.ReturnDate}";
            }

            // Thông tin Thanh toán
            if (booking.Payments != null && booking.Payments.Any())
            {
                var payment = booking.Payments.First();
                txtPaymentInfo.Text = $"Số tiền: {payment.Amount:N0} VNĐ\n" +
                                      $"Ngày thanh toán: {payment.PaymentDate:dd/MM/yyyy}\n" +
                                      $"Phương thức: {payment.PaymentMethod ?? "N/A"}";
            }
            else
            {
                txtPaymentInfo.Text = "Chưa có thanh toán.";
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();
    }
}
