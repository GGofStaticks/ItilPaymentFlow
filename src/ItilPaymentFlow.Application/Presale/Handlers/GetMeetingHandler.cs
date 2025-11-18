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

internal sealed class GetMeetingHandler : IRequestHandler<GetMeetingQuery, MeetingDto?>
{
    private readonly IMeetingRepository _repo;
    public GetMeetingHandler(IMeetingRepository repo) => _repo = repo;

    public async Task<MeetingDto?> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
    {
        var m = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (m == null) return null;
        return new MeetingDto
        {
            Id = m.Id,
            At = m.At,
            Topic = m.Topic,
            Participants = m.Participants,
            FileUrl = m.FileUrl,
            Link = m.Link,
            OrganiserId = m.OrganiserId,
            CreatedAtUtc = m.CreatedAtUtc
        };
    }
}