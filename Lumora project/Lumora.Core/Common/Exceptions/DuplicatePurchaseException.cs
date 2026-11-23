namespace Lumora.Common.Exceptions
{
    public class DuplicatePurchaseException : Exception
    {
        public string StudentId { get; }
        public string CourseId { get; }

        public DuplicatePurchaseException(string studentId, string courseId)
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
