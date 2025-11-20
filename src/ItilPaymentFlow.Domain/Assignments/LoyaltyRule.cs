using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItilPaymentFlow.Domain.Abstractions;

namespace ItilPaymentFlow.Domain.Assignments
{
    public sealed class LoyaltyRule : AggregateRoot<Guid>
    {
        private LoyaltyRule() { }

        public LoyaltyRule(Guid id, string name, int pointsForAssignmentType, int levelThreshold)
        {
            Id = id;
            Name = name;
            PointsForAssignmentType = pointsForAssignmentType;
            LevelThreshold = levelThreshold;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static LoyaltyRule Create(string name, int pointsForAssignmentType, int levelThreshold)
            => new LoyaltyRule(Guid.NewGuid(), name?.Trim() ?? string.Empty, pointsForAssignmentType, levelThreshold);

        public string Name { get; private set; } = string.Empty;
        public int PointsForAssignmentType { get; private set; } // бонусные очки для особых заданий
        public int LevelThreshold { get; private set; } // необходимое количество очков для этого уровня
        public DateTime CreatedAtUtc { get; private set; }

        public void Update(string name, int pointsForAssignmentType, int levelThreshold)
        {
            Name = name;
            PointsForAssignmentType = pointsForAssignmentType;
            LevelThreshold = levelThreshold;
        }
    }
}
