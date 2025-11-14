using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Domain.ValueObjects
{
    // Value object для идентификатора пользователя.
    // Реализован как класс (immutable) с явной конверсией в Guid/из Guid.
    public sealed class UserId : IEquatable<UserId>
    {
        public Guid Value { get; }

        public UserId(Guid value) => Value = value;

        public static UserId New() => new UserId(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public override bool Equals(object? obj) => Equals(obj as UserId);

        public bool Equals(UserId? other) => other is not null && other.Value == Value;

        public override int GetHashCode() => Value.GetHashCode();

        // явные/неявные конверсии для упрощения сравнений
        public static implicit operator Guid(UserId id) => id.Value;
        public static explicit operator UserId(Guid g) => new UserId(g);
    }
}