namespace ItilPaymentFlow.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

