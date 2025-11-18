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

internal sealed class GetContractHandler : IRequestHandler<GetContractQuery, ContractDto?>
{
    private readonly IContractRepository _repo;
    public GetContractHandler(IContractRepository repo) => _repo = repo;

    public async Task<ContractDto?> Handle(GetContractQuery request, CancellationToken cancellationToken)
    {
        var c = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (c == null) return null;
        return new ContractDto
        {
            Id = c.Id,
            Title = c.Title,
            Number = c.Number,
            StartAt = c.StartAt,
            EndAt = c.EndAt,
            FileUrl = c.FileUrl,
            CounterpartyId = c.CounterpartyId,
            CreatedAtUtc = c.CreatedAtUtc
        };
    }
}