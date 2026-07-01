using System.Windows;

namespace Presentation
{
    public partial class CustomerWindow : Window
    {
        public CustomerWindow()
        {
            InitializeComponent();
            txtWelcome.Text = $"Xin chào, {SessionManager.CurrentAccount?.Username}!";
            MainFrame.Navigate(new TourListPage());
        }

        private void btnNavTours_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new TourListPage());
        private void btnNavBookTour_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new BookTourPage());
        private void btnNavMyBookings_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new MyBookingsPage());

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.Clear();
            new LoginWindow().Show();
            this.Close();
        }
    }
}
