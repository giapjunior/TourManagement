using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace Presentation
{
    public partial class SchedulePage : Page
    {
        private readonly ScheduleService _scheduleService;
        private readonly TourService _tourService;
        private bool _isLoaded = false;

        public SchedulePage()
        {
            var context = new TourManagementDbContext();
            var tourRepo = new TourRepository(context);
            _scheduleService = new ScheduleService(new ScheduleRepository(context), tourRepo);
            _tourService = new TourService(tourRepo);
            InitializeComponent();

            // Set _isLoaded trước khi set ItemsSource để tránh loop
            _isLoaded = true;
            var tours = _tourService.GetActive();
            tours.Insert(0, new Tour { TourId = 0, TourName = "-- Tất cả --" });
            cboFilterTour.ItemsSource = tours;
            cboFilterTour.SelectedIndex = 0;
            // LoadData được gọi tự động qua cboFilterTour_Changed khi SelectedIndex = 0
        }

        /// <summary>
        /// Load dữ liệu lịch trình, lọc theo Tour nếu được chọn
        /// </summary>
        private void LoadData()
        {
            if (!_isLoaded || dgSchedule == null) return;
            var selectedTourId = cboFilterTour?.SelectedValue != null ? (int)cboFilterTour.SelectedValue : 0;
            dgSchedule.ItemsSource = selectedTourId > 0
                ? _scheduleService.GetByTourId(selectedTourId)
                : _scheduleService.GetAll();
        }

        private void cboFilterTour_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoaded) LoadData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ScheduleDialog();
            if (dialog.ShowDialog() == true) LoadData();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            var schedule = _scheduleService.GetById(id);
            var dialog = new ScheduleDialog(schedule);
            if (dialog.ShowDialog() == true) LoadData();
        }

        /// <summary>
        /// Hủy lịch khởi hành: đổi Status = "Cancelled"
        /// Cảnh báo nếu có booking Confirmed
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            if (MessageBox.Show("Bạn có chắc muốn hủy lịch khởi hành này?",
                "Xác nhận hủy", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    var warning = _scheduleService.CancelSchedule(id);
                    if (!string.IsNullOrEmpty(warning))
                        MessageBox.Show(warning, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    else
                        MessageBox.Show("Lịch trình đã được hủy.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            if (MessageBox.Show("Bạn có chắc muốn xóa lịch trình này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try { _scheduleService.Delete(id); LoadData(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }
    }
}
