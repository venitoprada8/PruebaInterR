using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Repositories;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Repositories;

public class InscriptionRepository : GenericRepository<Inscription>, IInscriptionRepository
{
    public InscriptionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Inscription>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Subject)
                .ThenInclude(s => s.Teacher)
            .Where(i => i.StudentId == studentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Inscription>> GetBySubjectIdAsync(Guid subjectId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Student)
            .Where(i => i.SubjectId == subjectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Student>> GetClassmatesAsync(Guid studentId, Guid subjectId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(i => i.SubjectId == subjectId && i.StudentId != studentId)
            .Select(i => i.Student)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsStudentInscribedInSubjectAsync(Guid studentId, Guid subjectId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(
            i => i.StudentId == studentId && i.SubjectId == subjectId,
            cancellationToken);
    }
}
