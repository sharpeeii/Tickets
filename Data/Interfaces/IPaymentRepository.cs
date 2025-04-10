using Data.Entities;

namespace Data.Interfaces;

public interface IPaymentRepository
{
    public Task CreatePaymentAsync(PaymentSimulation payment);
    public Task<ICollection<PaymentSimulation>> GetAllPaymentsAsync(int pageIndex, int pageSize = 5);
    public Task<ICollection<PaymentSimulation>> GetUsersPaymentsAsync(Guid userId, int pageIndex, int pageSize = 5);
    public Task<PaymentSimulation?> GetPaymentSimulation(Guid paymentId);
}