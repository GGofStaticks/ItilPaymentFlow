using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Domain.Abstractions;
using MediatR;

namespace ItilPaymentFlow.Infrastructure.Persistence;

internal sealed class UnitOfWork(PaymentDbContext context, IMediator mediator) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await context.SaveChangesAsync(cancellationToken);

        var domainEntities = context.ChangeTracker
            .Entries()
            .Select(e => e.Entity)
            .OfType<IHasDomainEvents>()
            .ToList();

        foreach (var entity in domainEntities)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }
            entity.ClearDomainEvents();
        }

        return result;
    }
}