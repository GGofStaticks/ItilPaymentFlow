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

internal sealed class CreateContractHandler : IRequestHandler<CreateContractCommand, ContractDto>
{
    private readonly IContractRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMinioService _minio;

    public CreateContractHandler(IContractRepository repo, IUnitOfWork uow, IMinioService minio)
    {
        _repo = repo;
        _uow = uow;
        _minio = minio;
    }

    public async Task<ContractDto> Handle(CreateContractCommand request, CancellationToken cancellationToken)
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
                    var fileName = $"contract_{Guid.NewGuid():N}.bin";
                    url = await _minio.UploadFileAsync(fileName, ms, "application/octet-stream");
                }
                catch (FormatException)
                {
                    // пропуск некорректной строки
                }
            }
        }

        var entity = Contract.Create(request.Title, request.Number, request.StartAt, request.EndAt, url, request.CounterpartyId);
        await _repo.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new ContractDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Number = entity.Number,
            StartAt = entity.StartAt,
            EndAt = entity.EndAt,
            FileUrl = entity.FileUrl,
            CounterpartyId = entity.CounterpartyId,
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }
}