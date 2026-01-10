using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Domain.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student?> GetByIdWithInscriptionsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> GetAllWithInscriptionsAsync(CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(string email, Guid? excludeStudentId = null, CancellationToken cancellationToken = default);
}
