using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Presale.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Queries
{
    public sealed record GetMeetingQuery(Guid Id) : IRequest<MeetingDto?>;
}
