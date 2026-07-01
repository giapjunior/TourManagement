using DataAccess.DataAccess;

namespace Service
{
    public interface IScheduleService
    {
        List<Schedule> GetAll();
        List<Schedule> GetAvailable();
        List<Schedule> GetByTourId(int tourId);
        Schedule GetById(int id);
        void Add(Schedule schedule);
        void Update(Schedule schedule);
        void Delete(int id);
        // Hủy lịch khởi hành — kiểm tra booking Confirmed trước
        string CancelSchedule(int scheduleId);
    }
}
