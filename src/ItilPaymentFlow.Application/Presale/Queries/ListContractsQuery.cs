using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Presale.Common;
using ItilPaymentFlow.Application.Presale.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Queries;

public sealed record ListContractsQuery : IRequest<PagedResult<ContractDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public ListContractsQuery() { }
}