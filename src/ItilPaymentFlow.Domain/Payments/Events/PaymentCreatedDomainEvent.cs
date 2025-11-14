using MediatR;
using ItilPaymentFlow.Domain.ValueObjects;

namespace ItilPaymentFlow.Domain.Payments.Events;

public sealed record PaymentCreatedDomainEvent(PaymentId PaymentId) : INotification;

