using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Repositories;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Repositories;

public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Subject?> GetByIdWithTeacherAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Teacher)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Subject>> GetAllWithTeachersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Teacher)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Subject>> GetByTeacherIdAsync(Guid teacherId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.TeacherId == teacherId)
            .ToListAsync(cancellationToken);
    }
}
