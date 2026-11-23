using Lumora.Models;

namespace Lumora.Repositories.Interfaces
{
    public interface ICertificateRepository : IGenericRepository<Certificate>
    {
        Task<Certificate?> GetByVerificationIdAsync(string verificationId);
        Task<IReadOnlyList<Certificate>> GetCertificatesByStudentAsync(string studentId);
    }
}
