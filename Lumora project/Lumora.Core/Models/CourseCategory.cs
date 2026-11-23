using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class CourseCategory
    {
        public string CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
