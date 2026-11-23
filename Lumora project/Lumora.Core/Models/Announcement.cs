using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Announcement
    {
        public string AnnouncementId { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime PublishedAt { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; } = null!;
    }
}
