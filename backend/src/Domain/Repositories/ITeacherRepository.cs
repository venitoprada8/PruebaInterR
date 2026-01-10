using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Domain.Repositories;

public interface ITeacherRepository : IGenericRepository<Teacher>
{
    Task<Teacher?> GetByIdWithSubjectsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Teacher>> GetAllWithSubjectsAsync(CancellationToken cancellationToken = default);
}
