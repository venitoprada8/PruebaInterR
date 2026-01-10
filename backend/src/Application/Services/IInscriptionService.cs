using StudentRegistration.Application.DTOs;

namespace StudentRegistration.Application.Services;

public interface IInscriptionService
{
    Task<StudentDto> InscribeStudentAsync(CreateInscriptionDto dto, CancellationToken cancellationToken = default);
    Task UnscribeStudentAsync(Guid studentId, Guid subjectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InscriptionDto>> GetStudentInscriptionsAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentListDto>> GetSubjectStudentsAsync(Guid subjectId, CancellationToken cancellationToken = default);
}
