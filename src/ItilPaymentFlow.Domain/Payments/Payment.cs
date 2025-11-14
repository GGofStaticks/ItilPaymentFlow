using ItilPaymentFlow.Domain.Abstractions;
using ItilPaymentFlow.Domain.Payments.Events;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Domain.Payments;

public sealed class Payment : AggregateRoot<PaymentId>
{
    private Payment() {}

    public string Reference { get; private set; } = string.Empty;
    public Money Amount { get; private set; } = null!;
    public PaymentStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public string? FailureReason { get; private set; }

    public static Payment Create(Money amount, string reference, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Reference is required", nameof(reference));

        var payment = new Payment
        {
            Id = PaymentId.New(),
            Amount = amount,
            Reference = reference.Trim(),
            Status = PaymentStatus.Created,
            CreatedAtUtc = utcNow
        };

        payment.Raise(new PaymentCreatedDomainEvent(payment.Id));
        return payment;
    }

    public void Complete(DateTime utcNow)
    {
        if (Status != PaymentStatus.Created)
            throw new InvalidOperationException("Only created payments can be completed");

        Status = PaymentStatus.Completed;
        CompletedAtUtc = utcNow;
        Raise(new PaymentCompletedDomainEvent(Id));
    }

    public void Fail(string reason)
    {
        if (Status != PaymentStatus.Created)
            throw new InvalidOperationException("Only created payments can be failed");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Failure reason is required", nameof(reason));

        Status = PaymentStatus.Failed;
        FailureReason = reason.Trim();
        Raise(new PaymentFailedDomainEvent(Id, FailureReason));
    }
}

