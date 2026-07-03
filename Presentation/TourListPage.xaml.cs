using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Presentation
{
    public partial class TourListPage : Page
    {
        private readonly TourService _tourService;

        public TourListPage()
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            _tourService = new TourService(new TourRepository(context));
            LoadData();
        }

        private void LoadData()
        {
            var keyword = txtSearch.Text.Trim();
            decimal? maxPrice = decimal.TryParse(txtMaxPrice.Text, out decimal price) ? price : null;
            System.DateTime? departureDate = dpDepartureDate.SelectedDate;

            if (string.IsNullOrWhiteSpace(keyword) && maxPrice == null && departureDate == null)
            {
                dgTour.ItemsSource = _tourService.GetActive();
            }
            else
            {
                dgTour.ItemsSource = _tourService.AdvancedSearch(keyword, maxPrice, departureDate);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) => LoadData();
        private void txtSearch_KeyUp(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LoadData(); }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            var tourId = (int)((Button)sender).Tag;
            var window = Window.GetWindow(this) as CustomerWindow;
            window?.MainFrame.Navigate(new BookTourPage(tourId));
        }
    }
}
