using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Tag
    {
        public string TagId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();
    }
}
