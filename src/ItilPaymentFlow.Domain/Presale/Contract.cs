using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;

namespace ItilPaymentFlow.Domain.Presale;

public sealed class Contract : AggregateRoot<Guid>
{
    private Contract() { }

    public Contract(
        Guid id,
        string title,
        string number,
        DateTime startAt,
        DateTime endAt,
        string? fileUrl,
        Guid counterpartyId)
    {
        Id = id;
        Title = title;
        Number = number;
        StartAt = startAt;
        EndAt = endAt;
        FileUrl = fileUrl;
        CounterpartyId = counterpartyId;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Contract Create(string title, string number, DateTime startAt, DateTime endAt, string? fileUrl, Guid counterpartyId)
        => new Contract(Guid.NewGuid(), title?.Trim() ?? string.Empty, number ?? string.Empty, startAt, endAt, fileUrl, counterpartyId);

    public string Title { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public string? FileUrl { get; private set; }
    public Guid CounterpartyId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public void SetFileUrl(string url) => FileUrl = url;
}