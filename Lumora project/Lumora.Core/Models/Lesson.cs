using System;
using System.Collections.Generic;
using System.Text;
namespace Lumora.Models
{
    public class Lesson
    {
        public string LessonId { get; set; }
        public string Title { get; set; } = null!;
        public int SortOrder { get; set; }
        public string CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string SectionId { get; set; }
        public Section Section { get; set; } = null!; public ICollection<MediaAsset> MediaAssets { get; set; } = new List<MediaAsset>();
        public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
