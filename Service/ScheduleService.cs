using DataAccess.DataAccess;
using Repository;

namespace Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepo;
        private readonly ITourRepository _tourRepo;

        public ScheduleService(IScheduleRepository scheduleRepo, ITourRepository tourRepo)
        {
            _scheduleRepo = scheduleRepo;
            _tourRepo = tourRepo;
        }

        public List<Schedule> GetAll() => _scheduleRepo.GetAll();

        public List<Schedule> GetAvailable() => _scheduleRepo.GetAvailable();

        public List<Schedule> GetByTourId(int tourId) => _scheduleRepo.GetByTourId(tourId);

        public Schedule GetById(int id) => _scheduleRepo.GetById(id);

        public void Add(Schedule schedule)
        {
            // Validate ngày: ngày khởi hành phải trước ngày về
            if (schedule.DepartureDate >= schedule.ReturnDate)
                throw new Exception("Ngày khởi hành phải trước ngày về.");
            // Ngày khởi hành phải là ngày trong tương lai
            if (schedule.DepartureDate <= DateOnly.FromDateTime(DateTime.Today))
                throw new Exception("Ngày khởi hành phải là ngày trong tương lai.");
            if (schedule.AvailableSlot <= 0)
                throw new Exception("Số chỗ trống phải lớn hơn 0.");

            // Kiểm tra AvailableSlot không vượt quá MaxSlot của Tour
            var tour = _tourRepo.GetById(schedule.TourId);
            if (tour != null && schedule.AvailableSlot > tour.MaxSlot)
                throw new Exception($"Số chỗ trống không được vượt quá {tour.MaxSlot} (MaxSlot của Tour).");

            _scheduleRepo.Add(schedule);
        }

        public void Update(Schedule schedule)
        {
            if (schedule.DepartureDate >= schedule.ReturnDate)
                throw new Exception("Ngày khởi hành phải trước ngày về.");
            if (schedule.AvailableSlot < 0)
                throw new Exception("Số chỗ trống không được âm.");

            // Kiểm tra AvailableSlot không vượt quá MaxSlot
            var tour = _tourRepo.GetById(schedule.TourId);
            if (tour != null && schedule.AvailableSlot > tour.MaxSlot)
                throw new Exception($"Số chỗ trống không được vượt quá {tour.MaxSlot} (MaxSlot của Tour).");

            _scheduleRepo.Update(schedule);
        }

        public void Delete(int id) => _scheduleRepo.Delete(id);

        /// <summary>
        /// Hủy lịch khởi hành:
        /// - Kiểm tra có booking Confirmed nào không → cảnh báo
        /// - Đổi Status = "Cancelled"
        /// </summary>
        public string CancelSchedule(int scheduleId)
        {
            var schedule = _scheduleRepo.GetByIdWithBookings(scheduleId);
            if (schedule == null)
                throw new Exception("Lịch trình không tồn tại.");

            // Kiểm tra booking đang Confirmed
            var confirmedCount = schedule.Bookings.Count(b => b.Status == "Confirmed");
            string warning = "";
            if (confirmedCount > 0)
                warning = $"Cảnh báo: Có {confirmedCount} booking đã xác nhận sẽ bị ảnh hưởng.";

            schedule.Status = "Cancelled";
            _scheduleRepo.Update(schedule);

            return warning;
        }
    }
}
