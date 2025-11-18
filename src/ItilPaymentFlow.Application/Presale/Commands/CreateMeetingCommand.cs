using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Presale.DTOs;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Commands
{
    public sealed record CreateMeetingCommand(
        DateTime At,
        string Topic,
        string Participants,
        string? File, 
        string? Link,
        Guid OrganiserId
    ) : IRequest<MeetingDto>;
}
