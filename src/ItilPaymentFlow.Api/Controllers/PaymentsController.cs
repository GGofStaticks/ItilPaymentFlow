using FluentValidation;
using ItilPaymentFlow.Application.Payments.Commands.CreatePayment;
using ItilPaymentFlow.Application.Payments.Queries.GetPaymentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItilPaymentFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IValidator<CreatePaymentCommand> _validator;

    public PaymentsController(ISender sender, IValidator<CreatePaymentCommand> validator)
    {
        _sender = sender;
        _validator = validator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _sender.Send(new GetPaymentByIdQuery(id));
        if (!result.IsSuccess) return NotFound(new { error = result.Error });
        return Ok(result.Value);
    }

    public sealed record CreatePaymentRequest(decimal Amount, string Currency, string Reference);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
    {
        var command = new CreatePaymentCommand(request.Amount, request.Currency, request.Reference);
        var validation = await _validator.ValidateAsync(command);
        if (!validation.IsValid)
        {
            var errs = validation.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new ValidationProblemDetails(errs));
        }

        var result = await _sender.Send(command);
        if (!result.IsSuccess) return BadRequest(new { error = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }
}
