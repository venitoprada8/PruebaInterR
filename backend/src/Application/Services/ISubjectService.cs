using StudentRegistration.Application.DTOs;

namespace StudentRegistration.Application.Services;

public interface ISubjectService
{
    Task<SubjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubjectListDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SubjectListDto>> GetAvailableSubjectsForStudentAsync(Guid studentId, CancellationToken cancellationToken = default);
}
