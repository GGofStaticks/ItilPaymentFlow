using ItilPaymentFlow.Domain.Abstractions;

namespace ItilPaymentFlow.Domain.ValueObjects;

public sealed class PaymentId : ValueObject
{
    public Guid Value { get; }

    private PaymentId(Guid value)
    {
        Value = value == Guid.Empty ? throw new ArgumentException("Id cannot be empty", nameof(value)) : value;
    }

    public static PaymentId New() => new(Guid.NewGuid());
    public static PaymentId From(Guid value) => new(value);
    public static implicit operator Guid(PaymentId id) => id.Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}

