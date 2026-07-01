using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;

namespace Presentation
{
    public partial class ScheduleDialog : Window
    {
        private readonly ScheduleService _scheduleService;
        private readonly TourService _tourService;
        private Schedule? _editing;

        public ScheduleDialog(Schedule? schedule = null)
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            var tourRepo = new TourRepository(context);
            _scheduleService = new ScheduleService(new ScheduleRepository(context), tourRepo);
            _tourService = new TourService(tourRepo);
            _editing = schedule;

            cboTour.ItemsSource = _tourService.GetActive();

            if (schedule != null)
            {
                txtTitle.Text = "Sửa Lịch trình";
                cboTour.SelectedValue = schedule.TourId;
                dpDeparture.SelectedDate = schedule.DepartureDate.ToDateTime(TimeOnly.MinValue);
                dpReturn.SelectedDate = schedule.ReturnDate.ToDateTime(TimeOnly.MinValue);
                txtSlot.Text = schedule.AvailableSlot.ToString();
                foreach (System.Windows.Controls.ComboBoxItem item in cboStatus.Items)
                    if (item.Content.ToString() == schedule.Status) { cboStatus.SelectedItem = item; break; }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate dữ liệu đầu vào
            if (cboTour.SelectedValue == null) { MessageBox.Show("Chọn tour."); return; }
            if (dpDeparture.SelectedDate == null || dpReturn.SelectedDate == null) { MessageBox.Show("Chọn ngày đi và ngày về."); return; }
            if (!int.TryParse(txtSlot.Text, out int slot)) { MessageBox.Show("Số chỗ không hợp lệ."); return; }

            try
            {
                var status = (cboStatus.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString() ?? "Open";

                if (_editing == null)
                {
                    _scheduleService.Add(new Schedule
                    {
                        TourId = (int)cboTour.SelectedValue,
                        DepartureDate = DateOnly.FromDateTime(dpDeparture.SelectedDate.Value),
                        ReturnDate = DateOnly.FromDateTime(dpReturn.SelectedDate.Value),
                        AvailableSlot = slot,
                        Status = status
                    });
                }
                else
                {
                    _editing.TourId = (int)cboTour.SelectedValue;
                    _editing.DepartureDate = DateOnly.FromDateTime(dpDeparture.SelectedDate.Value);
                    _editing.ReturnDate = DateOnly.FromDateTime(dpReturn.SelectedDate.Value);
                    _editing.AvailableSlot = slot;
                    _editing.Status = status;
                    _scheduleService.Update(_editing);
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
