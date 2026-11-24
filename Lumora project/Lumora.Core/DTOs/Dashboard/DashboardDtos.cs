namespace Lumora.DTOs.Dashboard
{
    /// <summary>
    /// صحن واحد بيجمع كل البيانات اللي الـ Dashboard محتاجها
    /// بدل ما نروح لكل جدول على حدة
    /// </summary>
    public class DashboardDto
    {
        // ── أرقام الـ KPIs ──────────────────────────────────────────────────
        public int    TotalStudents        { get; set; }
        public int    TotalInstructors     { get; set; }
        public int    TotalPublishedCourses { get; set; }
        public int    TotalEnrollments     { get; set; }
        public decimal TotalRevenue        { get; set; }
        public int    CertificatesIssued   { get; set; }
        public int    PendingApprovalCount { get; set; }

        // ── آخر 10 أوردرات ──────────────────────────────────────────────────
        public List<RecentOrderDto>      RecentOrders      { get; set; } = new();

        // ── أحسن 5 كورسات (enrollment) ──────────────────────────────────────
        public List<TopCourseDto>        TopCourses        { get; set; } = new();

        // ── آخر 10 enrollments ──────────────────────────────────────────────
        public List<RecentEnrollmentDto> RecentEnrollments { get; set; } = new();

        // ── إيرادات آخر 6 شهور (للـ chart) ─────────────────────────────────
        public List<MonthlyRevenueDto>   MonthlyRevenue    { get; set; } = new();
    }

    public class RecentOrderDto
    {
        public string   OrderId     { get; set; } = string.Empty;
        public string   StudentName { get; set; } = string.Empty;
        public decimal  TotalAmount { get; set; }
        public string   Currency    { get; set; } = "EGP";
        public string   Status      { get; set; } = string.Empty;
        public DateTime CreatedAt   { get; set; }
    }

    public class TopCourseDto
    {
        public string CourseId        { get; set; } = string.Empty;
        public string Title           { get; set; } = string.Empty;
        public string InstructorName  { get; set; } = string.Empty;
        public int    EnrollmentCount { get; set; }
        public double AverageRating   { get; set; }
        public decimal Revenue        { get; set; }
    }

    public class RecentEnrollmentDto
    {
        public string   StudentName { get; set; } = string.Empty;
        public string   CourseTitle { get; set; } = string.Empty;
        public DateTime EnrolledAt  { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public int     Year       { get; set; }
        public int     Month      { get; set; }
        public string  MonthLabel => new DateTime(Year, Month, 1).ToString("MMM yyyy");
        public decimal Revenue    { get; set; }
        public int     OrderCount { get; set; }
    }
}
