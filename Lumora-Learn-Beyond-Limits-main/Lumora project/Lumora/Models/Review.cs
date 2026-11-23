using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public ICollection<ReviewVote> Votes { get; set; } = new List<ReviewVote>();
    }
}
