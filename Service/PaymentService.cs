using DataAccess.DataAccess;
using Repository;

namespace Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IBookingRepository _bookingRepo;

        public PaymentService(IPaymentRepository paymentRepo, IBookingRepository bookingRepo = null)
        {
            _paymentRepo = paymentRepo;
            _bookingRepo = bookingRepo;
        }

        public List<Payment> GetAll() => _paymentRepo.GetAll();

        public Payment GetById(int id) => _paymentRepo.GetById(id);

        public Payment? GetByBookingId(int bookingId) => _paymentRepo.GetByBookingId(bookingId);

        public List<Payment> GetByCustomerId(int customerId) => _paymentRepo.GetByCustomerId(customerId);

        // Lấy Payment kèm Booking → Schedule → Tour cho thống kê
        public List<Payment> GetAllWithDetails() => _paymentRepo.GetAllWithDetails();

        public void ProcessPayment(int bookingId, string method)
        {
            if (_bookingRepo == null) throw new System.Exception("BookingRepository is required to process payment.");
            var booking = _bookingRepo.GetById(bookingId);
            if (booking == null) throw new System.Exception("Booking không tồn tại.");
            if (booking.Status != "Pending") throw new System.Exception("Chỉ có thể thanh toán cho booking Pending.");
            
            var existingPayment = _paymentRepo.GetByBookingId(bookingId);
            if (existingPayment != null) throw new System.Exception("Booking này đã được thanh toán.");

            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = booking.TotalPrice,
                PaymentDate = System.DateTime.Now,
                PaymentMethod = method
            };
            _paymentRepo.Add(payment);

            booking.Status = "Confirmed";
            _bookingRepo.Update(booking);
        }
    }
}
