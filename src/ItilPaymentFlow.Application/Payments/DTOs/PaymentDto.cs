using ItilPaymentFlow.Domain.Payments;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Application.Payments.DTOs;

public sealed record PaymentDto(string Id, string Reference, decimal Amount, string Currency, string Status, DateTime CreatedAtUtc, DateTime? CompletedAtUtc, string? FailureReason)
{
    public static PaymentDto FromDomain(Payment payment) => new(
        payment.Id.Value.ToString(),
        payment.Reference,
        payment.Amount.Amount,
        payment.Amount.Currency,
        payment.Status.ToString(),
        payment.CreatedAtUtc,
        payment.CompletedAtUtc,
        payment.FailureReason);
}

