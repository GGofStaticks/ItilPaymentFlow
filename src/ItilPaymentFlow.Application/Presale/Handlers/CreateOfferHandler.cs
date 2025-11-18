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

internal sealed class CreateOfferHandler : IRequestHandler<CreateOfferCommand, OfferDto>
{
    private readonly IOfferRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMinioService _minio;

    public CreateOfferHandler(IOfferRepository repo, IUnitOfWork uow, IMinioService minio)
    {
        _repo = repo;
        _uow = uow;
        _minio = minio;
    }

    public async Task<OfferDto> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        string? url = null;

        if (!string.IsNullOrWhiteSpace(request.File))
        {
            // если это правильный урл, то сохраняется как есть
            if (Uri.IsWellFormedUriString(request.File, UriKind.Absolute))
            {
                url = request.File;
            }
            else
            {
                // читаем как бейз 64 и загрузка в минио
                try
                {
                    var bytes = Convert.FromBase64String(request.File);
                    using var ms = new MemoryStream(bytes);
                    var fileName = $"offer_{Guid.NewGuid():N}.bin";
                    url = await _minio.UploadFileAsync(fileName, ms, "application/octet-stream");
                }
                catch (FormatException)
                {
                    // пропуск некорректной строки
                }
            }
        }

        var offer = Offer.Create(request.Title, request.Amount, request.Number, request.ValidUntil, url, request.SupplierId);

        await _repo.AddAsync(offer, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new OfferDto
        {
            Id = offer.Id,
            Title = offer.Title,
            Amount = offer.Amount,
            Number = offer.Number,
            ValidUntil = offer.ValidUntil,
            FileUrl = offer.FileUrl,
            SupplierId = offer.SupplierId,
            Status = offer.Status.ToString(),
            CreatedAtUtc = offer.CreatedAtUtc
        };
    }
}