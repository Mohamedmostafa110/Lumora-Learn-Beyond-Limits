using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public AccountStatus AccountStatus { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public ICollection<ApprovalEvent> ApprovalEvents { get; set; } = new List<ApprovalEvent>();
    }
}
