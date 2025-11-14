using MediatR;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Domain.Payments.Events;

public sealed record PaymentCompletedDomainEvent(PaymentId PaymentId) : INotification;

