using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
    }
}
