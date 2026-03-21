using guest_house_management_backend.Models;

namespace guest_house_management_backend.Repositories.PaymentRepo
{
    public interface IPaymentRepository
    {
        public Task createpayment(Payment payment);
    }
}
