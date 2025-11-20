using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItilPaymentFlow.Application.Assignments.DTOs
{
    public sealed class SummaryDto
    {
        public int TotalPoints { get; set; }
        public int PointsThisMonth { get; set; }
        public int PointsToNextLevel { get; set; }
    }
}
