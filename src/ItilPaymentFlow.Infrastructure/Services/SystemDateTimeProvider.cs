using ItilPaymentFlow.Application.Abstractions;

namespace ItilPaymentFlow.Infrastructure.Services;

internal sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}