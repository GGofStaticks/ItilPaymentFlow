using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Presale.Common;
using ItilPaymentFlow.Application.Presale.DTOs;
using ItilPaymentFlow.Application.Presale.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ItilPaymentFlow.Application.Presale.Handlers;

internal sealed class ListOffersHandler : IRequestHandler<ListOffersQuery, PagedResult<OfferDto>>
{
    private readonly IOfferRepository _repo;

    public ListOffersHandler(IOfferRepository repo) => _repo = repo;

    public async Task<PagedResult<OfferDto>> Handle(ListOffersQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.Query().AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(o => o.CreatedAtUtc)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(o => new OfferDto
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
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<OfferDto>(items, request.Page, request.PageSize, totalCount);
    }
}