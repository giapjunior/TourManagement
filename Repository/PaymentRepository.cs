using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly TourManagementDbContext _context;

        public PaymentRepository(TourManagementDbContext context)
        {
            _context = context;
        }

        public List<Payment> GetAll() => _context.Payments.ToList();

        public Payment GetById(int id) => _context.Payments.FirstOrDefault(p => p.PaymentId == id);

        public void Add(Payment entity)
        {
            _context.Payments.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Payment entity)
        {
            _context.Payments.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Payments.Remove(entity);
                _context.SaveChanges();
            }
        }

        public Payment? GetByBookingId(int bookingId)
            => _context.Payments.FirstOrDefault(p => p.BookingId == bookingId);

        // Include Booking → Schedule → Tour cho thống kê doanh thu
        public List<Payment> GetAllWithDetails()
            => _context.Payments
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Schedule)
                        .ThenInclude(s => s.Tour)
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Customer)
                .ToList();
    }
}
