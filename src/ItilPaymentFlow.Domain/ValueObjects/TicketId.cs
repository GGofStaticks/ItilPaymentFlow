using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;

namespace ItilPaymentFlow.Domain.ValueObjects
{
    public sealed class TicketId : ValueObject
    {
        public Guid Value { get; }

        private TicketId(Guid value)
        {
            Value = value == Guid.Empty ? throw new ArgumentException("Id cannot be empty", nameof(value)) : value;
        }

        public static TicketId New() => new TicketId(Guid.NewGuid());
        public static TicketId From(Guid value) => new TicketId(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}