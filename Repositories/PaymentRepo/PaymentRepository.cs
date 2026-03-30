//using guest_house_management_backend.Data;
//using guest_house_management_backend.Models;

//namespace guest_house_management_backend.Repositories.PaymentRepo
//{
//    public class PaymentRepository : IPaymentRepository
//    {
//        private readonly DBContext _context;

//        public PaymentRepository(DBContext context)
//        {
//            _context = context;
//        }

//        public async Task createpayment(Payment payment)
//        {
//            await _context.Payments.AddAsync(payment);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
