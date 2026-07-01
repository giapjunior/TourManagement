using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation
{
    public partial class TourPage : Page
    {
        private readonly TourService _tourService;
        private bool _isLoaded = false; // cờ ngăn event trigger trước khi page sẵn sàng

        public TourPage()
        {
            // Khởi tạo service TRƯỚC InitializeComponent
            var context = new TourManagementDbContext();
            _tourService = new TourService(new TourRepository(context));
            InitializeComponent();
            _isLoaded = true;
            LoadData();
        }

        /// <summary>
        /// Load dữ liệu tour, hỗ trợ tìm kiếm và sắp xếp bằng LINQ
        /// </summary>
        private void LoadData(string keyword = "")
        {
            // Guard: chỉ chạy sau khi InitializeComponent hoàn tất
            if (!_isLoaded || dgTour == null) return;

            var tours = string.IsNullOrWhiteSpace(keyword)
                ? _tourService.GetAll()
                : _tourService.Search(keyword);

            // Sắp xếp bằng LINQ OrderBy / OrderByDescending
            var sortIndex = cboSort?.SelectedIndex ?? 0;
            tours = sortIndex switch
            {
                1 => tours.OrderBy(t => t.Price).ToList(),
                2 => tours.OrderByDescending(t => t.Price).ToList(),
                3 => tours.OrderBy(t => t.TourName).ToList(),
                4 => tours.OrderByDescending(t => t.TourName).ToList(),
                _ => tours
            };

            dgTour.ItemsSource = tours;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) => LoadData(txtSearch.Text);
        private void txtSearch_KeyUp(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LoadData(txtSearch.Text); }
        private void cboSort_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Chỉ load khi page đã sẵn sàng
            if (_isLoaded) LoadData(txtSearch?.Text ?? "");
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TourDialog();
            if (dialog.ShowDialog() == true) LoadData();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            var tour = _tourService.GetById(id);
            var dialog = new TourDialog(tour);
            if (dialog.ShowDialog() == true) LoadData();
        }

        /// <summary>
        /// Xóa mềm (soft delete): set IsActive = false thay vì xóa cứng
        /// </summary>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).Tag;
            if (MessageBox.Show("Bạn có chắc muốn ẩn tour này? (Tour sẽ không bị xóa khỏi hệ thống)",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _tourService.SoftDelete(id);
                    LoadData();
                    MessageBox.Show("Tour đã được ẩn thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
