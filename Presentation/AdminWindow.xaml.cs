using System.Windows;

namespace Presentation
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new TourPage());
        }

        private void btnNavTour_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new TourPage());
        private void btnNavSchedule_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new SchedulePage());
        private void btnNavBooking_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new BookingPage());
        private void btnNavCustomer_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CustomerPage());
        private void btnNavAccount_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new AccountPage());
        private void btnNavStats_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new StatisticsPage());

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.Clear();
            new LoginWindow().Show();
            this.Close();
        }
    }
}
