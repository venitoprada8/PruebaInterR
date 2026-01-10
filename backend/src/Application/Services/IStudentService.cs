using StudentRegistration.Application.DTOs;

namespace StudentRegistration.Application.Services;

public interface IStudentService
{
    Task<StudentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentListDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StudentDto> CreateAsync(CreateStudentDto dto, CancellationToken cancellationToken = default);
    Task<StudentDto> UpdateAsync(Guid id, UpdateStudentDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClassmateDto>> GetStudentClassmatesAsync(Guid studentId, CancellationToken cancellationToken = default);
}
