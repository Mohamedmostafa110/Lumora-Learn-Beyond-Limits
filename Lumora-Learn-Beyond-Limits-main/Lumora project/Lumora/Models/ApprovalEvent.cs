using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class ApprovalEvent
    {
        public int ApprovalEventId { get; set; }
        public ApprovalDecision Decision { get; set; }
        public DateTime ReviewedAt { get; set; }
        public int AdminId { get; set; }
        public Admin Admin { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
