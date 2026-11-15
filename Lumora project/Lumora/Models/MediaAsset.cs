using System;
using System.Collections.Generic;
using System.Text;

namespace Lumora.Models
{
    public class MediaAsset
    {
        public int MediaAssetId { get; set; }
        public string StorageKey { get; set; } = null!;
        public string? ThumbnailKey { get; set; }
        public MediaProcessingStatus ProcessingStatus { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
