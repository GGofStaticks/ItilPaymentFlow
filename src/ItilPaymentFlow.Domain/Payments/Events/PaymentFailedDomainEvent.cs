using MediatR;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Domain.Payments.Events;

public sealed record PaymentFailedDomainEvent(PaymentId PaymentId, string Reason) : INotification;

