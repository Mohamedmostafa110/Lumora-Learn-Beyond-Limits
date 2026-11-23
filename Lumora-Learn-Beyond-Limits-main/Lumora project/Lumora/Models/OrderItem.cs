using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class OrderItem
    {
        public int ItemId { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }

}
