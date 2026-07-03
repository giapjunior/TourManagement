using DataAccess.DataAccess;
using Repository;

namespace Service
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepo;

        public TourService(ITourRepository tourRepo)
        {
            _tourRepo = tourRepo;
        }

        public List<Tour> GetAll() => _tourRepo.GetAll();

        public List<Tour> GetActive() => _tourRepo.GetActive();

        public Tour GetById(int id) => _tourRepo.GetById(id);

        public List<Tour> Search(string keyword) => _tourRepo.Search(keyword);

        public List<Tour> AdvancedSearch(string keyword, decimal? maxPrice, System.DateTime? departureDate) 
            => _tourRepo.AdvancedSearch(keyword, maxPrice, departureDate);

        public void Add(Tour tour)
        {
            // Validate dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(tour.TourName))
                throw new Exception("Tên tour không được để trống.");
            if (string.IsNullOrWhiteSpace(tour.Destination))
                throw new Exception("Điểm đến không được để trống.");
            if (tour.Price <= 0)
                throw new Exception("Giá tour phải lớn hơn 0.");
            if (tour.MaxSlot <= 0)
                throw new Exception("Số chỗ tối đa phải lớn hơn 0.");
            _tourRepo.Add(tour);
        }

        public void Update(Tour tour)
        {
            if (string.IsNullOrWhiteSpace(tour.TourName))
                throw new Exception("Tên tour không được để trống.");
            if (string.IsNullOrWhiteSpace(tour.Destination))
                throw new Exception("Điểm đến không được để trống.");
            if (tour.Price <= 0)
                throw new Exception("Giá tour phải lớn hơn 0.");
            _tourRepo.Update(tour);
        }

        /// <summary>
        /// Xóa mềm: set IsActive = false thay vì xóa cứng khỏi DB
        /// (vì Tour có thể đã có Booking liên quan)
        /// </summary>
        public void SoftDelete(int id)
        {
            var tour = _tourRepo.GetById(id);
            if (tour == null)
                throw new Exception("Tour không tồn tại.");
            tour.IsActive = false;
            _tourRepo.Update(tour);
        }
    }
}
