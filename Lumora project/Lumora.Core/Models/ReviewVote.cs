using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class ReviewVote
    {
        public string ReviewVoteId { get; set; }
        public bool Helpful { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; } = null!;
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
    }
}
