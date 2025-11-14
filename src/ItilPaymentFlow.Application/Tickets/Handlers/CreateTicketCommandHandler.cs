using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Abstractions.Storage;
using ItilPaymentFlow.Application.Tickets.DTOs;
using ItilPaymentFlow.Domain.Tickets;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ItilPaymentFlow.Application.Tickets.Commands.CreateTicket
{
    internal sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMinioService _minioService;

        public CreateTicketCommandHandler(
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMinioService minioService)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _minioService = minioService;
        }

        public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var createdAt = DateTime.UtcNow;
            string? attachmentsAsString = null;

            if (request.Attachments is { Count: > 0 })
            {
                var uploadedFiles = new List<string>();
                foreach (var attachment in request.Attachments)
                {
                    if (Uri.IsWellFormedUriString(attachment, UriKind.Absolute))
                    {
                        uploadedFiles.Add(attachment);
                        continue;
                    }

                    try
                    {
                        var bytes = Convert.FromBase64String(attachment);
                        using var stream = new System.IO.MemoryStream(bytes);
                        var fileName = $"attachment_{Guid.NewGuid()}.bin";
                        var url = await _minioService.UploadFileAsync(fileName, stream, "application/octet-stream");
                        uploadedFiles.Add(url);
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                }

                attachmentsAsString = JsonSerializer.Serialize(uploadedFiles);
            }

            var lastTicket = await _ticketRepository.Query()
                .OrderByDescending(t => t.CreatedAtUtc)
                .FirstOrDefaultAsync(cancellationToken);

            int nextNumberValue = 1;
            if (lastTicket != null && !string.IsNullOrWhiteSpace(lastTicket.Number))
            {
                var m = Regex.Match(lastTicket.Number ?? string.Empty, @"(\d+)$");
                if (m.Success && int.TryParse(m.Value, out var parsed))
                    nextNumberValue = parsed + 1;
            }

            var nextNumber = $"REQ-{nextNumberValue:D3}"; // REQ-001 итд

            var ticket = Ticket.Create(
                request.Title,
                request.Priority,
                request.Description,
                request.Contacts,
                attachmentsAsString,
                createdAt,
                nextNumber,
                request.AuthorId);

            await _ticketRepository.AddAsync(ticket, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var user = await _userRepository.GetByIdAsync(request.AuthorId, cancellationToken);

            return new TicketDto
            {
                Id = ticket.Id,
                Number = ticket.Number,
                Title = ticket.Title,
                Priority = ticket.Priority,
                Description = ticket.Description,
                Contacts = ticket.Contacts,
                Attachments = ticket.Attachments,
                CreatedAtUtc = ticket.CreatedAtUtc,
                SlaTime = ticket.SlaTime,
                Status = ticket.Status.ToString(),
                Author = user == null ? null : new AuthorDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                }
            };
        }
    }
}