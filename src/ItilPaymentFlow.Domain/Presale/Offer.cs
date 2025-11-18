using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;

namespace ItilPaymentFlow.Domain.Presale;

public sealed class Offer : AggregateRoot<Guid>
{
    private Offer() { }

    public Offer(
        Guid id,
        string title,
        decimal amount,
        string number,
        DateTime validUntil,
        string? fileUrl,
        Guid supplierId)
    {
        Id = id;
        Title = title;
        Amount = amount;
        Number = number;
        ValidUntil = validUntil;
        FileUrl = fileUrl;
        SupplierId = supplierId;
        Status = OfferStatus.OnReview;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Offer Create(string title, decimal amount, string number, DateTime validUntil, string? fileUrl, Guid supplierId)
        => new Offer(Guid.NewGuid(), title?.Trim() ?? string.Empty, amount, number ?? string.Empty, validUntil, fileUrl, supplierId);

    public string Title { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Number { get; private set; } = string.Empty;
    public DateTime ValidUntil { get; private set; }
    public string? FileUrl { get; private set; }
    public Guid SupplierId { get; private set; }
    public OfferStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public void SetStatus(OfferStatus status) => Status = status;
    public void SetFileUrl(string url) => FileUrl = url;
}