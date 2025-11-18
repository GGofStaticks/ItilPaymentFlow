using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Presale.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Commands
{
    public sealed record CreateOfferCommand(
        string Title,
        decimal Amount,
        string Number,
        DateTime ValidUntil,
        string? File, // юрл или бейз64
        Guid SupplierId
    ) : IRequest<OfferDto>;
}