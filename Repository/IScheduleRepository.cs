using DataAccess.DataAccess;

namespace Repository
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        List<Schedule> GetByTourId(int tourId);
        List<Schedule> GetAvailable();
        // Lấy schedule kèm Bookings để kiểm tra trước khi hủy
        List<Schedule> GetByTourIdWithBookings(int tourId);
        // Lấy schedule kèm Bookings theo ID
        Schedule? GetByIdWithBookings(int scheduleId);
    }
}
