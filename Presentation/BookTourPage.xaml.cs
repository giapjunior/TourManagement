using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace Presentation
{
    public partial class BookTourPage : Page
    {
        private readonly BookingService _bookingService;
        private readonly ScheduleService _scheduleService;
        private Schedule? _selectedSchedule;

        public BookTourPage(int? preselectTourId = null)
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            _scheduleService = new ScheduleService(new ScheduleRepository(context), new TourRepository(context));
            _bookingService = new BookingService(new BookingRepository(context), new ScheduleRepository(context));
            
            var schedules = _scheduleService.GetAvailable();
            if (preselectTourId.HasValue)
            {
                var tour = new TourRepository(context).GetById(preselectTourId.Value);
                if (tour != null)
                {
                    pnlTourInfo.Visibility = Visibility.Visible;
                    txtTourName.Text = tour.TourName;
                    txtTourDestination.Text = tour.Destination;
                    txtTourPrice.Text = $"{tour.Price:N0} VNĐ";
                    txtTourDescription.Text = tour.Description;
                }
                
                schedules = schedules.Where(s => s.TourId == preselectTourId.Value).ToList();
                
                if (schedules.Count == 0)
                {
                    txtNoScheduleMsg.Visibility = Visibility.Visible;
                }
            }
            dgSchedule.ItemsSource = schedules;
        }

        private void dgSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSchedule = dgSchedule.SelectedItem as Schedule;
            if (_selectedSchedule != null)
            {
                txtSelectedTour.Text = $"{_selectedSchedule.Tour.TourName} - {_selectedSchedule.DepartureDate} → {_selectedSchedule.ReturnDate}";
                UpdateTotal();
            }
        }

        private void txtPeople_TextChanged(object sender, TextChangedEventArgs e) => UpdateTotal();

        private void UpdateTotal()
        {
            if (_selectedSchedule == null) return;
            if (int.TryParse(txtPeople.Text, out int n) && n > 0)
                txtTotal.Text = $"Tổng tiền: {(_selectedSchedule.Tour.Price * n):N0} VNĐ";
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSchedule == null) { MessageBox.Show("Vui lòng chọn lịch trình!"); return; }
            if (!int.TryParse(txtPeople.Text, out int n) || n <= 0) { MessageBox.Show("Số người không hợp lệ!"); return; }

            var customer = SessionManager.CurrentCustomer;
            if (customer == null) { MessageBox.Show("Không tìm thấy thông tin khách hàng!"); return; }

            try
            {
                _bookingService.Add(new Booking
                {
                    CustomerId = customer.CustomerId,
                    ScheduleId = _selectedSchedule.ScheduleId,
                    NumberOfPeople = n
                });
                MessageBox.Show("Đặt tour thành công! Vui lòng chờ xác nhận.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                dgSchedule.ItemsSource = null;
                dgSchedule.ItemsSource = _scheduleService.GetAvailable();
                txtSelectedTour.Text = "(Chọn lịch trình)";
                txtPeople.Text = "1";
                _selectedSchedule = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
