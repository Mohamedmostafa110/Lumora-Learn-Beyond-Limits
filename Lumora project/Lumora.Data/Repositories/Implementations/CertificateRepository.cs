using Lumora.Data;
using Lumora.Models;
using Lumora.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lumora.Repositories.Implementations
{
    public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Certificate?> GetByVerificationIdAsync(string verificationId)
        {
            return await _dbSet.AsNoTracking()
                .Include(c => c.Student)
                .Include(c => c.Course)
                    .ThenInclude(course => course.Instructor)
                .FirstOrDefaultAsync(c => c.VerificationId == verificationId);
        }

        public async Task<IReadOnlyList<Certificate>> GetCertificatesByStudentAsync(int studentId)
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.StudentId == studentId)
                .Include(c => c.Course)
                .OrderByDescending(c => c.IssuedAt)
                .ToListAsync();
        }
    }
}
