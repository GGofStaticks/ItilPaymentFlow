using FluentValidation;
using ItilPaymentFlow.Application.Abstractions;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Common;
using ItilPaymentFlow.Application.Payments.DTOs;
using ItilPaymentFlow.Domain.Payments;
using ItilPaymentFlow.Domain.ValueObjects;
using MediatR;

namespace ItilPaymentFlow.Application.Payments.Commands.CreatePayment;

public sealed record CreatePaymentCommand(decimal Amount, string Currency, string Reference) : IRequest<Result<PaymentDto>>;

public sealed class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.Reference).NotEmpty().MaximumLength(64);
    }
}

public sealed class CreatePaymentCommandHandler( IPaymentRepository repository, IUnitOfWork unitOfWork, IDateTimeProvider clock) : IRequestHandler<CreatePaymentCommand, Result<PaymentDto>>
{
    public async Task<Result<PaymentDto>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var amount = Money.From(request.Amount, request.Currency);
        var payment = Payment.Create(amount, request.Reference, clock.UtcNow);

        await repository.Add(payment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<PaymentDto>.Success(PaymentDto.FromDomain(payment));
    }
}
