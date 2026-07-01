using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly TourManagementDbContext _context;

        public ScheduleRepository(TourManagementDbContext context)
        {
            _context = context;
        }

        public List<Schedule> GetAll() => _context.Schedules.Include(s => s.Tour).ToList();

        public Schedule GetById(int id) => _context.Schedules.Include(s => s.Tour).FirstOrDefault(s => s.ScheduleId == id);

        public void Add(Schedule entity)
        {
            _context.Schedules.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Schedule entity)
        {
            _context.Schedules.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Schedules.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<Schedule> GetByTourId(int tourId)
            => _context.Schedules.Include(s => s.Tour).Where(s => s.TourId == tourId).ToList();

        // Sửa lại dùng "Open" thay vì "Active" theo đúng schema DB
        public List<Schedule> GetAvailable()
            => _context.Schedules.Include(s => s.Tour)
                .Where(s => s.AvailableSlot > 0 && s.Status == "Open")
                .ToList();

        // Include Bookings để kiểm tra trước khi hủy lịch
        public List<Schedule> GetByTourIdWithBookings(int tourId)
            => _context.Schedules
                .Include(s => s.Tour)
                .Include(s => s.Bookings)
                .Where(s => s.TourId == tourId)
                .ToList();

        // Lấy 1 schedule kèm Bookings theo ID
        public Schedule? GetByIdWithBookings(int scheduleId)
            => _context.Schedules
                .Include(s => s.Tour)
                .Include(s => s.Bookings)
                .FirstOrDefault(s => s.ScheduleId == scheduleId);
    }
}
