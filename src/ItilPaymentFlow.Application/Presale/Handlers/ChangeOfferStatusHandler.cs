using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Application.Abstractions.Persistence;
using ItilPaymentFlow.Application.Presale.Commands;
using ItilPaymentFlow.Domain.Presale;
using MediatR;

namespace ItilPaymentFlow.Application.Presale.Handlers;

internal sealed class ChangeOfferStatusHandler : IRequestHandler<ChangeOfferStatusCommand, Unit>
{
    private readonly IOfferRepository _repo;
    private readonly IUnitOfWork _uow;

    public ChangeOfferStatusHandler(IOfferRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(ChangeOfferStatusCommand request, CancellationToken cancellationToken)
    {
        var offer = await _repo.GetByIdAsync(request.OfferId, cancellationToken)
                    ?? throw new KeyNotFoundException("Offer not found");

        offer.SetStatus(request.NewStatus);
        await _repo.UpdateAsync(offer, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}