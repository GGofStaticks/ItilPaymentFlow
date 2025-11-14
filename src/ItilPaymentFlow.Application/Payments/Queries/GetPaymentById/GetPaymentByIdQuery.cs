using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Common;
using ItilPaymentFlow.Application.Payments.DTOs;
using ItilPaymentFlow.Domain.ValueObjects;
using MediatR;

namespace ItilPaymentFlow.Application.Payments.Queries.GetPaymentById;

public sealed record GetPaymentByIdQuery(string Id) : IRequest<Result<PaymentDto>>;

public sealed class GetPaymentByIdQueryHandler(IPaymentRepository repository)
    : IRequestHandler<GetPaymentByIdQuery, Result<PaymentDto>>
{
    public async Task<Result<PaymentDto>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id, out var guid))
            return Result<PaymentDto>.Failure("Invalid id format");

        var payment = await repository.GetByIdAsync(PaymentId.From(guid), cancellationToken);
        if (payment is null)
            return Result<PaymentDto>.Failure("Payment not found");

        return Result<PaymentDto>.Success(PaymentDto.FromDomain(payment));
    }
}

