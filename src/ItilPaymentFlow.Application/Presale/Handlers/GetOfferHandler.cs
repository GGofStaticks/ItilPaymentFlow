using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Presale.DTOs;
using ItilPaymentFlow.Application.Presale.Queries;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Handlers;

internal sealed class GetOfferHandler : IRequestHandler<GetOfferQuery, OfferDto?>
{
    private readonly IOfferRepository _repo;

    public GetOfferHandler(IOfferRepository repo) => _repo = repo;

    public async Task<OfferDto?> Handle(GetOfferQuery request, CancellationToken cancellationToken)
    {
        var o = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (o == null) return null;
        return new OfferDto
        {
            Id = o.Id,
            Title = o.Title,
            Amount = o.Amount,
            Number = o.Number,
            ValidUntil = o.ValidUntil,
            FileUrl = o.FileUrl,
            SupplierId = o.SupplierId,
            Status = o.Status.ToString(),
            CreatedAtUtc = o.CreatedAtUtc
        };
    }
}