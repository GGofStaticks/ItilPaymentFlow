using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Presale.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Commands
{
    public sealed record CreateContractCommand(
        string Title,
        string Number,
        DateTime StartAt,
        DateTime EndAt,
        string? File,
        Guid CounterpartyId
    ) : IRequest<ContractDto>;
}
