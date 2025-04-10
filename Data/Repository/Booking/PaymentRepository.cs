using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unit;

    public PaymentRepository(AppDbContext context, IUnitOfWork unit)
    {
        _context = context;
        _unit = unit;
    }

public async Task CreatePaymentAsync(PaymentSimulation payment)
{
    await _context.Payments.AddAsync(payment);
    await _context.SaveChangesAsync();
}

    public async Task<ICollection<PaymentSimulation>> GetAllPaymentsAsync(int pageIndex, int pageSize = 5)
    {
        ICollection<PaymentSimulation> allPayments = await _context.Payments
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return allPayments;
    }

    public async Task<ICollection<PaymentSimulation>> GetUsersPaymentsAsync(Guid userId, int pageIndex, int pageSize = 5)
    {
        ICollection<PaymentSimulation> allUsersPayments = await _context.Payments
            .Where(p => p.UserId == userId)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return allUsersPayments;
    }

    public async Task<PaymentSimulation?> GetPaymentSimulation(Guid paymentId)
    {
        PaymentSimulation? payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        return payment;
    }
}