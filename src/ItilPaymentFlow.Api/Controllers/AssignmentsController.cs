using ItilPaymentFlow.Application.Assignments.Commands;
using ItilPaymentFlow.Application.Assignments.Queries;
using ItilPaymentFlow.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItilPaymentFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class AssignmentsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ItilPaymentFlow.Application.Abstractions.Storage.IMinioService _minio;

        public AssignmentsController(ISender sender, ItilPaymentFlow.Application.Abstractions.Storage.IMinioService minio)
        {
            _sender = sender;
            _minio = minio;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListAssignmentsQuery query, CancellationToken ct)
        {
            var result = await _sender.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var dto = await _sender.Send(new GetAssignmentQuery(id), ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // создание заданий админская опция
        public async Task<IActionResult> Create([FromBody] CreateAssignmentCommand cmd, CancellationToken ct)
        {
            var dto = await _sender.Send(cmd, ct);
            return Ok(dto);
        }

        [HttpPost("{id:guid}/submit")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Submit(Guid id, [FromForm] SubmitAssignmentRequest request, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim is null) return Unauthorized();
            var userId = Guid.Parse(userIdClaim);

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

            var cmd = new SubmitAssignmentCommand(id, userId, request.SubmissionText, fileArg);
            var res = await _sender.Send(cmd, ct);
            return Ok(res);
        }

        [HttpGet("completed")]
        public async Task<IActionResult> ListCompleted([FromQuery] ListCompletedAssignmentsQuery query, CancellationToken ct)
        {
            var result = await _sender.Send(query, ct);
            return Ok(result);
        }

        [HttpPut("completed/{id:guid}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(Guid id, [FromBody] ApproveCompletedAssignmentRequest request, CancellationToken ct)
        {
            var cmd = new ApproveCompletedAssignmentCommand(id, request.AwardedPoints, request.Approve);
            await _sender.Send(cmd, ct);
            return Ok();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> Summary(CancellationToken ct)
        {
            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userIdClaim is null) return Unauthorized();
            var userId = Guid.Parse(userIdClaim);

            var dto = await _sender.Send(new GetSummaryQuery(userId), ct);
            return Ok(dto);
        }

        [HttpPost("loyalty-rules")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLoyaltyRule(CreateLoyaltyRuleCommand command, CancellationToken ct)
        {
            var result = await _sender.Send(command, ct);
            return Ok(result);
        }

        [HttpGet("loyalty-rules")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListLoyaltyRules([FromQuery] ListLoyaltyRulesQuery query, CancellationToken ct)
        {
            var result = await _sender.Send(query, ct);
            return Ok(result);
        }

        // реквест модели
        public sealed class SubmitAssignmentRequest
        {
            public string? SubmissionText { get; set; }
            public IFormFile? File { get; set; }
        }

        public sealed class ApproveCompletedAssignmentRequest
        {
            public int AwardedPoints { get; set; }
            public bool Approve { get; set; } = true;
        }
    }
}
