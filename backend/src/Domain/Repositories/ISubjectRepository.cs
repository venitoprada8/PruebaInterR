using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Domain.Repositories;

public interface ISubjectRepository : IGenericRepository<Subject>
{
    Task<Subject?> GetByIdWithTeacherAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subject>> GetAllWithTeachersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Subject>> GetByTeacherIdAsync(Guid teacherId, CancellationToken cancellationToken = default);
}
