using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Presale;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Commands
{
    public sealed record ChangeOfferStatusCommand(Guid OfferId, OfferStatus NewStatus) : IRequest<Unit>;
}