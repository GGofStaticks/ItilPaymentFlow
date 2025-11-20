using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;

namespace ItilPaymentFlow.Domain.Assignments
{
    public sealed class Assignment : AggregateRoot<Guid>
    {
        private Assignment() { }

        public Assignment(Guid id, string title, string shortDescription, int points, bool active)
        {
            Id = id;
            Title = title;
            ShortDescription = shortDescription;
            Points = points;
            Active = active;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static Assignment Create(string title, string shortDescription, int points, bool active = true)
            => new Assignment(Guid.NewGuid(), title?.Trim() ?? string.Empty, shortDescription?.Trim() ?? string.Empty, points, active);

        public string Title { get; private set; } = string.Empty;
        public string ShortDescription { get; private set; } = string.Empty;
        public int Points { get; private set; }
        public bool Active { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        public void Deactivate() => Active = false;
        public void Activate() => Active = true;
        public void Update(string title, string shortDescription, int points, bool active)
        {
            Title = title;
            ShortDescription = shortDescription;
            Points = points;
            Active = active;
        }
    }
}
