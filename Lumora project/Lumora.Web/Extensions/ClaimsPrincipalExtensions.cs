using System.Security.Claims;

namespace Lumora.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(ClaimTypes.NameIdentifier);

        public static string GetFullName(this ClaimsPrincipal principal) =>
            principal.FindFirstValue("FullName")
            ?? principal.Identity?.Name
            ?? "User";

        public static string GetInitials(this ClaimsPrincipal principal)
        {
            var name = principal.GetFullName().Trim();
            if (string.IsNullOrEmpty(name))
            {
                return "LU";
            }

            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                return $"{char.ToUpperInvariant(parts[0][0])}{char.ToUpperInvariant(parts[1][0])}";
            }

            return name.Length >= 2
                ? name[..2].ToUpperInvariant()
                : name.ToUpperInvariant();
        }
    }
}
