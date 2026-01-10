using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Domain.Repositories;

public interface IInscriptionRepository : IGenericRepository<Inscription>
{
    Task<IEnumerable<Inscription>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Inscription>> GetBySubjectIdAsync(Guid subjectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> GetClassmatesAsync(Guid studentId, Guid subjectId, CancellationToken cancellationToken = default);
    Task<bool> IsStudentInscribedInSubjectAsync(Guid studentId, Guid subjectId, CancellationToken cancellationToken = default);
}
