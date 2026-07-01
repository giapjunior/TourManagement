using DataAccess.DataAccess;

namespace Service
{
    public interface IPaymentService
    {
        List<Payment> GetAll();
        Payment GetById(int id);
        Payment? GetByBookingId(int bookingId);
        List<Payment> GetAllWithDetails();
    }
}
