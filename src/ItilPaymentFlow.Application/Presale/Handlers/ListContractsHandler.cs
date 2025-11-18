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

internal sealed class ListContractsHandler : IRequestHandler<ListContractsQuery, PagedResult<ContractDto>>
{
    private readonly IContractRepository _repo;

    public ListContractsHandler(IContractRepository repo) => _repo = repo;

    public async Task<PagedResult<ContractDto>> Handle(ListContractsQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.Query().AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(c => c.CreatedAtUtc)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new ContractDto
            {
                Id = c.Id,
                Title = c.Title,
                Number = c.Number,
                StartAt = c.StartAt,
                EndAt = c.EndAt,
                FileUrl = c.FileUrl,
                CounterpartyId = c.CounterpartyId,
                CreatedAtUtc = c.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ContractDto>(items, request.Page, request.PageSize, totalCount);
    }
}