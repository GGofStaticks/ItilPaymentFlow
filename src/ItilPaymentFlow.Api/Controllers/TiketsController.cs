using ItilPaymentFlow.Application.Abstractions;
using ItilPaymentFlow.Application.Abstractions.Storage;
using ItilPaymentFlow.Application.Tickets.Commands.CreateTicket;
using ItilPaymentFlow.Application.Tickets.Queries.GetTicket;
using ItilPaymentFlow.Application.Tickets.Queries.ListTickets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ItilPaymentFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class TicketsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMinioService _minio;

        public TicketsController(ISender sender, IMinioService minio)
        {
            _sender = sender;
            _minio = minio;
        }

        // форма с полями в свагере
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateTicketRequest request, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userIdClaim is null)
                return Unauthorized();

            var authorId = Guid.Parse(userIdClaim);

            // если есть файлы, то грузим в минио и получаем юрлы
            List<string>? attachmentsUrls = null;

            if (request.AttachmentsFiles != null && request.AttachmentsFiles.Count > 0)
            {
                var streams = new List<Stream>();
                var names = new List<string>();
                try
                {
                    foreach (var f in request.AttachmentsFiles)
                    {
                        streams.Add(f.OpenReadStream());
                        // добавка уникальности к имени файла
                        names.Add($"{Guid.NewGuid():N}_{Path.GetFileName(f.FileName)}");
                    }

                    attachmentsUrls = await _minio.UploadFileAsync(streams, names, ct);
                }
                finally
                {
                    // закрытие потоков
                    foreach (var s in streams) s.Dispose();
                }
            }

            // вызов команды, передаем список юрлов, если есть
            var dto = await _sender.Send(
                new CreateTicketCommand(
                    request.Title,
                    request.Priority,
                    request.Description,
                    request.Contacts,
                    attachmentsUrls, authorId), ct);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListTicketsQuery query, CancellationToken ct)
        {
            var list = await _sender.Send(query, ct);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken ct)
        {
            var dto = await _sender.Send(new GetTicketQuery(id), ct);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        // форма мультипарт dto
        public sealed class CreateTicketRequest
        {
            [FromForm(Name = "Title")]
            public string Title { get; set; } = string.Empty;

            [FromForm(Name = "Priority")]
            public int Priority { get; set; }

            [FromForm(Name = "Description")]
            public string Description { get; set; } = string.Empty;

            [FromForm(Name = "Contacts")]
            public string Contacts { get; set; } = string.Empty;

            // сюда свагер положит файлы
            [FromForm(Name = "AttachmentsFiles")]
            public IFormFileCollection? AttachmentsFiles { get; set; }
        }
    }
}