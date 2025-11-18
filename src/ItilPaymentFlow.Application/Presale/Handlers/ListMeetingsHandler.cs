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

internal sealed class ListMeetingsHandler : IRequestHandler<ListMeetingsQuery, PagedResult<MeetingDto>>
{
    private readonly IMeetingRepository _repo;

    public ListMeetingsHandler(IMeetingRepository repo) => _repo = repo;

    public async Task<PagedResult<MeetingDto>> Handle(ListMeetingsQuery request, CancellationToken cancellationToken)
    {
        var query = _repo.Query().AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(m => m.At)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new MeetingDto
            {
                Id = m.Id,
                At = m.At,
                Topic = m.Topic,
                Participants = m.Participants,
                FileUrl = m.FileUrl,
                Link = m.Link,
                OrganiserId = m.OrganiserId,
                CreatedAtUtc = m.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<MeetingDto>(items, request.Page, request.PageSize, totalCount);
    }
}