using DataAccess.DataAccess;
using Repository;
using Service;
using System.Windows;

namespace Presentation
{
    public partial class TourDialog : Window
    {
        private readonly TourService _tourService;
        private Tour? _editingTour;

        public TourDialog(Tour? tour = null)
        {
            InitializeComponent();
            var context = new TourManagementDbContext();
            _tourService = new TourService(new TourRepository(context));
            _editingTour = tour;

            if (tour != null)
            {
                txtTitle.Text = "Sửa Tour";
                txtTourName.Text = tour.TourName;
                txtDestination.Text = tour.Destination;
                txtDescription.Text = tour.Description;
                txtPrice.Text = tour.Price.ToString();
                txtMaxSlot.Text = tour.MaxSlot.ToString();
                chkIsActive.IsChecked = tour.IsActive;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            { MessageBox.Show("Giá không hợp lệ."); return; }
            if (!int.TryParse(txtMaxSlot.Text, out int maxSlot))
            { MessageBox.Show("Số chỗ không hợp lệ."); return; }

            try
            {
                if (_editingTour == null)
                {
                    _tourService.Add(new Tour
                    {
                        TourName = txtTourName.Text.Trim(),
                        Destination = txtDestination.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        Price = price,
                        MaxSlot = maxSlot,
                        IsActive = chkIsActive.IsChecked == true
                    });
                }
                else
                {
                    _editingTour.TourName = txtTourName.Text.Trim();
                    _editingTour.Destination = txtDestination.Text.Trim();
                    _editingTour.Description = txtDescription.Text.Trim();
                    _editingTour.Price = price;
                    _editingTour.MaxSlot = maxSlot;
                    _editingTour.IsActive = chkIsActive.IsChecked == true;
                    _tourService.Update(_editingTour);
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
