using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation
{
    public partial class BookingPage : Page
    {
        private readonly BookingService _bookingService;
        private bool _isLoaded = false;

        public BookingPage()
        {
            var context = new TourManagementDbContext();
            _bookingService = new BookingService(new BookingRepository(context), new ScheduleRepository(context));
            InitializeComponent();
            _isLoaded = true;
            LoadData();
        }

        /// <summary>
        /// Load dữ liệu booking, hỗ trợ lọc theo trạng thái và tìm kiếm
        /// </summary>
        private void LoadData()
        {
            if (!_isLoaded || dgBooking == null) return;

            var keyword = txtSearch?.Text.Trim() ?? "";
            var statusItem = cboStatus?.SelectedItem as ComboBoxItem;
            var status = statusItem?.Content.ToString();

            List<Booking> bookings;
            if (!string.IsNullOrWhiteSpace(keyword))
                bookings = _bookingService.Search(keyword);
            else if (status != null && status != "Tất cả")
                bookings = _bookingService.GetByStatus(status);
            else
                bookings = _bookingService.GetAll();

            dgBooking.ItemsSource = bookings;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) => LoadData();
        private void txtSearch_KeyUp(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LoadData(); }
        private void cboStatus_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoaded) LoadData();
        }

        /// <summary>
        /// Xem chi tiết booking: mở dialog hiển thị đầy đủ thông tin
        /// </summary>
        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            var booking = _bookingService.GetById(id);
            if (booking != null)
            {
                var dialog = new BookingDetailDialog(booking);
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Xác nhận booking: Pending → Confirmed
        /// </summary>
        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            try
            {
                _bookingService.ConfirmBooking(id);
                MessageBox.Show("Booking đã được xác nhận.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Hoàn thành booking: Confirmed → Completed
        /// </summary>
        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            try
            {
                _bookingService.CompleteBooking(id);
                MessageBox.Show("Booking đã được đánh dấu hoàn thành.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Hủy booking: yêu cầu nhập CancelReason, hoàn trả chỗ
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            if (MessageBox.Show("Bạn có chắc muốn hủy booking này?", "Xác nhận hủy",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Hiển thị dialog nhập lý do hủy
                var reasonDialog = new CancelReasonDialog();
                if (reasonDialog.ShowDialog() == true)
                {
                    try
                    {
                        _bookingService.CancelBooking(id, reasonDialog.CancelReason);
                        MessageBox.Show("Booking đã được hủy và chỗ đã được hoàn trả.", "Thành công",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
