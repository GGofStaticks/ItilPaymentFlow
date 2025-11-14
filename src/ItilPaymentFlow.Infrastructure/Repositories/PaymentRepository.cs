using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Payments;
using ItilPaymentFlow.Domain.ValueObjects;
using ItilPaymentFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ItilPaymentFlow.Infrastructure.Repositories;

internal sealed class PaymentRepository(PaymentDbContext context) : IPaymentRepository
{
    public async Task Add(Payment payment)
    {
        await context.Payments.AddAsync(payment);
    }

    public Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken = default)
        => context.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}