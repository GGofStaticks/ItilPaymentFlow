using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;

namespace ItilPaymentFlow.Domain.Presale;

public sealed class Meeting : AggregateRoot<Guid>
{
    private Meeting() { }

    public Meeting(Guid id, DateTime at, string topic, string participants, string? fileUrl, string? link, Guid organiserId)
    {
        Id = id;
        At = at;
        Topic = topic;
        Participants = participants;
        FileUrl = fileUrl;
        Link = link;
        OrganiserId = organiserId;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Meeting Create(DateTime at, string topic, string participants, string? fileUrl, string? link, Guid organiserId)
        => new Meeting(Guid.NewGuid(), at, topic?.Trim() ?? string.Empty, participants?.Trim() ?? string.Empty, fileUrl, link, organiserId);

    public DateTime At { get; private set; }
    public string Topic { get; private set; } = string.Empty;
    public string Participants { get; private set; } = string.Empty;
    public string? FileUrl { get; private set; }
    public string? Link { get; private set; }
    public Guid OrganiserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public void SetFileUrl(string url) => FileUrl = url;
    public void SetLink(string link) => Link = link;
}