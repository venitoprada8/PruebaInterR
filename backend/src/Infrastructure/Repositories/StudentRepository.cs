using Microsoft.EntityFrameworkCore;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Repositories;
using StudentRegistration.Infrastructure.Data;

namespace StudentRegistration.Infrastructure.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetByIdWithInscriptionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Inscriptions)
                .ThenInclude(i => i.Subject)
                    .ThenInclude(s => s.Teacher)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Student>> GetAllWithInscriptionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Inscriptions)
                .ThenInclude(i => i.Subject)
                    .ThenInclude(s => s.Teacher)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeStudentId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(s => s.Email == email);

        if (excludeStudentId.HasValue)
        {
            query = query.Where(s => s.Id != excludeStudentId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }
}
