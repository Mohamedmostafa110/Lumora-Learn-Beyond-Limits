using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class CourseTag
    {
        public string CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
