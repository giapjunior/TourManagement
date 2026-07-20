using DataAccess.DataAccess;
using Repository;
using Service;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Presentation
{
    public partial class ReviewDialog : Window
    {
        private readonly int _tourId;

        public ReviewDialog(int tourId)
        {
            InitializeComponent();
            _tourId = tourId;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var customer = SessionManager.CurrentCustomer;
            if (customer == null)
            {
                MessageBox.Show("Bạn cần đăng nhập để đánh giá.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int rating = 5 - cboRating.SelectedIndex; // 5 -> index 0, 4 -> index 1, ...
            
            try
            {
                var context = new TourManagementDbContext();
                var reviewService = new ReviewService(new ReviewRepository(context));

                var review = new Review
                {
                    CustomerId = customer.CustomerId,
                    TourId = _tourId,
                    Rating = rating,
                    Comment = txtComment.Text.Trim()
                };

                reviewService.Add(review);
                
                MessageBox.Show("Cảm ơn bạn đã đánh giá Tour!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
