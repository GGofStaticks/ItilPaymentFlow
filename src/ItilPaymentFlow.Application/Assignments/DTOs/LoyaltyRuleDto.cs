using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Assignments.DTOs
{
    public sealed class LoyaltyRuleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PointsForAssignmentType { get; set; }
        public int LevelThreshold { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
