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

        private void LoadData(string keyword = "")
        {
            dgTour.ItemsSource = string.IsNullOrWhiteSpace(keyword)
                ? _tourService.GetActive()
                : _tourService.Search(keyword);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) => LoadData(txtSearch.Text);
        private void txtSearch_KeyUp(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LoadData(txtSearch.Text); }
    }
}
