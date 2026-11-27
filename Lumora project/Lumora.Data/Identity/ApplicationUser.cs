using Microsoft.AspNetCore.Identity;//  

namespace Lumora.Data.Identity;

/// <summary>
/// ASP.NET Core Identity user entity representing authenticated accounts across all platform roles. مهم جدا يا شباب
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty; 
    public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
    public DateTime? EmailVerifiedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //VIP 
    public Student? StudentProfile { get; set; }
    public Instructor? InstructorProfile { get; set; }
    public Admin? AdminProfile { get; set; }
}
