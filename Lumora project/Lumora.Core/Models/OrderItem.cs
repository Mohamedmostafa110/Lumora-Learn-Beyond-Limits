using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class OrderItem
    {
        public string ItemId { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public string CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }

}
