using ItilPaymentFlow.Application.Abstractions.Storage;
using ItilPaymentFlow.Application.Presale.Commands;
using ItilPaymentFlow.Application.Presale.DTOs;
using ItilPaymentFlow.Application.Presale.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItilPaymentFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PresaleController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ItilPaymentFlow.Application.Abstractions.Storage.IMinioService _minio;

    public PresaleController(IMediator mediator, ItilPaymentFlow.Application.Abstractions.Storage.IMinioService minio)
    {
        _mediator = mediator;
        _minio = minio;
    }

    // 1) комерческие предложения

    [HttpPost("offers")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateOffer([FromForm] CreateOfferRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        if (userIdClaim is null)
            return Unauthorized();

        var supplierId = Guid.Parse(userIdClaim);

        string? fileArg = null;
        if (request.File != null)
        {
            var streams = new List<System.IO.Stream>();
            try
            {
                streams.Add(request.File.OpenReadStream());
                var names = new List<string> { $"{Guid.NewGuid():N}_{Path.GetFileName(request.File.FileName)}" };
                var urls = await _minio.UploadFileAsync(streams, names, ct);
                fileArg = urls.FirstOrDefault();
            }
            finally
            {
                foreach (var s in streams) s.Dispose();
            }
        }

        var cmd = new CreateOfferCommand(
            request.Title,
            request.Amount,
            request.Number,
            request.ValidUntil,
            fileArg,
            supplierId
        );

        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpGet("offers")]
    public async Task<IActionResult> ListOffers([FromQuery] ListOffersQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("offers/{id:guid}")]
    public async Task<IActionResult> GetOffer(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOfferQuery(id), ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("offers/{id:guid}/status")]
    public async Task<IActionResult> ChangeOfferStatus(Guid id, [FromBody] ChangeOfferStatusRequest request, CancellationToken ct)
    {
        var cmd = new ChangeOfferStatusCommand(id, request.Status);
        await _mediator.Send(cmd, ct);
        return Ok();
    }

    // 2) договоры

    [HttpPost("contracts")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateContract([FromForm] CreateContractRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        if (userIdClaim is null)
            return Unauthorized();

        var counterpartyId = Guid.Parse(userIdClaim);

        string? fileArg = null;
        if (request.File != null)
        {
            var streams = new List<System.IO.Stream>();
            try
            {
                streams.Add(request.File.OpenReadStream());
                var names = new List<string> { $"{Guid.NewGuid():N}_{Path.GetFileName(request.File.FileName)}" };
                var urls = await _minio.UploadFileAsync(streams, names, ct);
                fileArg = urls.FirstOrDefault();
            }
            finally
            {
                foreach (var s in streams) s.Dispose();
            }
        }

        var cmd = new CreateContractCommand(
            request.Title,
            request.Number,
            request.StartAt,
            request.EndAt,
            fileArg,
            counterpartyId
        );

        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpGet("contracts")]
    public async Task<IActionResult> ListContracts([FromQuery] ListContractsQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("contracts/{id:guid}")]
    public async Task<IActionResult> GetContract(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetContractQuery(id), ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // 3) митингс

    [HttpPost("meetings")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateMeeting([FromForm] CreateMeetingRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        if (userIdClaim is null)
            return Unauthorized();

        var organiserId = Guid.Parse(userIdClaim);

        string? fileArg = null;
        if (request.File != null)
        {
            var streams = new List<System.IO.Stream>();
            try
            {
                streams.Add(request.File.OpenReadStream());
                var names = new List<string> { $"{Guid.NewGuid():N}_{Path.GetFileName(request.File.FileName)}" };
                var urls = await _minio.UploadFileAsync(streams, names, ct);
                fileArg = urls.FirstOrDefault();
            }
            finally
            {
                foreach (var s in streams) s.Dispose();
            }
        }

        var cmd = new CreateMeetingCommand(
            request.At,
            request.Topic,
            request.Participants,
            fileArg,
            request.Link,
            organiserId
        );

        var result = await _mediator.Send(cmd, ct);
        return Ok(result);
    }

    [HttpGet("meetings")]
    public async Task<IActionResult> ListMeetings([FromQuery] ListMeetingsQuery query, CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpGet("meetings/{id:guid}")]
    public async Task<IActionResult> GetMeeting(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMeetingQuery(id), ct);
        if (result == null) return NotFound();
        return Ok(result);
    }
}


// реквест модели
public sealed class CreateOfferRequest
{
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime ValidUntil { get; set; }
    public IFormFile? File { get; set; }
}

public sealed class ChangeOfferStatusRequest
{
    public ItilPaymentFlow.Domain.Presale.OfferStatus Status { get; set; }
}

public sealed class CreateContractRequest
{
    public string Title { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public IFormFile? File { get; set; }
}

public sealed class CreateMeetingRequest
{
    public DateTime At { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Participants { get; set; } = string.Empty;
    public IFormFile? File { get; set; }
    public string? Link { get; set; }
}