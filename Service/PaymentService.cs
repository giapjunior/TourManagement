using DataAccess.DataAccess;
using Repository;

namespace Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;

        public PaymentService(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public List<Payment> GetAll() => _paymentRepo.GetAll();

        public Payment GetById(int id) => _paymentRepo.GetById(id);

        public Payment? GetByBookingId(int bookingId) => _paymentRepo.GetByBookingId(bookingId);

        // Lấy Payment kèm Booking → Schedule → Tour cho thống kê
        public List<Payment> GetAllWithDetails() => _paymentRepo.GetAllWithDetails();
    }
}
