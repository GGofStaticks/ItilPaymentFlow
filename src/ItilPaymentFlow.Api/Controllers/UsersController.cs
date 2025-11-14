using ItilPaymentFlow.Application.Users.Commands.CreateUser;
using ItilPaymentFlow.Application.Users.Commands.DeleteUser;
using ItilPaymentFlow.Application.Users.Commands.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItilPaymentFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var id = await _mediator.Send(
            new CreateUserCommand(
                request.Email,
                request.Password,
                request.Role,
                request.FirstName,
                request.LastName,
                request.MiddleName
            ),
            ct
        );
        return CreatedAtAction(nameof(Create), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken ct)
    {
        var command = new UpdateUserCommand(id, request.Email, request.Password, request.Role);
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteUserCommand(id), ct);
        return NoContent();
    }

    public record CreateUserRequest(
        string Email,
        string Password,
        string? Role,
        string FirstName,
        string LastName,
        string? MiddleName
    );
    public record UpdateUserRequest(string? Email, string? Password, string? Role);
}