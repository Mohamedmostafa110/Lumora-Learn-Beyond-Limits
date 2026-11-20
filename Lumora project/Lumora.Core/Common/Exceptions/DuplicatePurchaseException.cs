namespace Lumora.Common.Exceptions
{
    public class DuplicatePurchaseException : Exception
    {
        public int StudentId { get; }
        public int CourseId { get; }

        public DuplicatePurchaseException(int studentId, int courseId)
            : base($"Student #{studentId} is already enrolled in Course #{courseId}.")
        {
            StudentId = studentId;
            CourseId = courseId;
        }

        public DuplicatePurchaseException(string message) : base(message)
        {
        }
    }
}
