using ItilPaymentFlow.Domain.Payments;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Application.Abstractions.Persistence
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken = default);
        Task Add(Payment payment);
    }
}