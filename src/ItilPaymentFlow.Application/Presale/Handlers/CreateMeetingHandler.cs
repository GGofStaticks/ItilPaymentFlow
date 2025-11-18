using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Abstractions.Storage;
using ItilPaymentFlow.Application.Presale.Commands;
using ItilPaymentFlow.Application.Presale.DTOs;
using ItilPaymentFlow.Domain.Presale;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Handlers;

internal sealed class CreateMeetingHandler : IRequestHandler<CreateMeetingCommand, MeetingDto>
{
    private readonly IMeetingRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMinioService _minio;

    public CreateMeetingHandler(IMeetingRepository repo, IUnitOfWork uow, IMinioService minio)
    {
        _repo = repo;
        _uow = uow;
        _minio = minio;
    }

    public async Task<MeetingDto> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        string? url = null;

        if (!string.IsNullOrWhiteSpace(request.File))
        {
            if (Uri.IsWellFormedUriString(request.File, UriKind.Absolute))
            {
                url = request.File;
            }
            else
            {
                try
                {
                    var bytes = Convert.FromBase64String(request.File);
                    using var ms = new MemoryStream(bytes);
                    var fileName = $"meeting_{Guid.NewGuid():N}.bin";
                    url = await _minio.UploadFileAsync(fileName, ms, "application/octet-stream");
                }
                catch (FormatException)
                {
                    // пропуск невалидной строки
                }
            }
        }

        var entity = Meeting.Create(request.At, request.Topic, request.Participants, url, request.Link, request.OrganiserId);
        await _repo.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new MeetingDto
        {
            Id = entity.Id,
            At = entity.At,
            Topic = entity.Topic,
            Participants = entity.Participants,
            FileUrl = entity.FileUrl,
            Link = entity.Link,
            OrganiserId = entity.OrganiserId,
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }
}