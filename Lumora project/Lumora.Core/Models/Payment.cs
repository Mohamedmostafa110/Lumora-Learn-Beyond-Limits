using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Payment
    {
        public string PaymentId { get; set; }
        public string ExternalId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public PaymentStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }
        public string Provider { get; set; } = null!;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
