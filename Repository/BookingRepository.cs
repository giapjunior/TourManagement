using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TourManagementDbContext _context;

        public BookingRepository(TourManagementDbContext context)
        {
            _context = context;
        }

        public List<Booking> GetAll() => _context.Bookings.ToList();

        public Booking GetById(int id) => _context.Bookings.FirstOrDefault(b => b.BookingId == id);

        public void Add(Booking entity)
        {
            _context.Bookings.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Booking entity)
        {
            _context.Bookings.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Bookings.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<Booking> GetByCustomerId(int customerId)
            => _context.Bookings
                .Include(b => b.Schedule).ThenInclude(s => s.Tour)
                .Include(b => b.Customer)
                .Include(b => b.Payments)
                .Where(b => b.CustomerId == customerId)
                .ToList();

        // Include tất cả navigation property cần thiết cho Admin xem danh sách
        public List<Booking> GetAllWithDetails()
            => _context.Bookings
                .Include(b => b.Schedule).ThenInclude(s => s.Tour)
                .Include(b => b.Customer)
                .Include(b => b.Payments)
                .ToList();

        // Include chi tiết booking (Customer, Schedule, Tour, Payments)
        public Booking GetWithDetails(int bookingId)
            => _context.Bookings
                .Include(b => b.Schedule).ThenInclude(s => s.Tour)
                .Include(b => b.Customer).ThenInclude(c => c.Account)
                .Include(b => b.Payments)
                .FirstOrDefault(b => b.BookingId == bookingId);

        // Lọc booking theo trạng thái bằng LINQ Where
        public List<Booking> GetByStatus(string status)
            => _context.Bookings
                .Include(b => b.Schedule).ThenInclude(s => s.Tour)
                .Include(b => b.Customer)
                .Include(b => b.Payments)
                .Where(b => b.Status == status)
                .ToList();

        // Tìm kiếm theo tên khách hàng hoặc mã booking (dùng LINQ)
        public List<Booking> SearchWithDetails(string keyword)
            => _context.Bookings
                .Include(b => b.Schedule).ThenInclude(s => s.Tour)
                .Include(b => b.Customer)
                .Include(b => b.Payments)
                .Where(b => b.Customer.FullName.Contains(keyword)
                         || b.BookingId.ToString().Contains(keyword))
                .ToList();
    }
}
