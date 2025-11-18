using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Presale;

namespace ItilPaymentFlow.Application.Abstractions.Persistence;

public interface IOfferRepository
{
    Task AddAsync(Offer offer, CancellationToken cancellationToken = default);
    Task UpdateAsync(Offer offer, CancellationToken cancellationToken = default);
    Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Offer>> ListAsync(CancellationToken cancellationToken = default);
    IQueryable<Offer> Query();
}