using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Instructor
    {
        public string InstructorId { get; set; }
        public string? UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public AccountStatus AccountStatus { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
