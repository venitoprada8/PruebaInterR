using StudentRegistration.Application.DTOs;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Repositories;

namespace StudentRegistration.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IInscriptionRepository _inscriptionRepository;

    public StudentService(
        IStudentRepository studentRepository,
        IInscriptionRepository inscriptionRepository)
    {
        _studentRepository = studentRepository;
        _inscriptionRepository = inscriptionRepository;
    }

    public async Task<StudentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdWithInscriptionsAsync(id, cancellationToken);

        if (student == null)
            return null;

        return MapToDto(student);
    }

    public async Task<IEnumerable<StudentListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var students = await _studentRepository.GetAllWithInscriptionsAsync(cancellationToken);

        return students.Select(s => new StudentListDto(
            s.Id,
            s.FirstName,
            s.LastName,
            s.Email,
            s.TotalCredits
        ));
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto, CancellationToken cancellationToken = default)
    {
        // Validate unique email
        if (!await _studentRepository.IsEmailUniqueAsync(dto.Email, null, cancellationToken))
        {
            throw new InvalidOperationException($"El correo {dto.Email} ya está registrado.");
        }

        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            RegistrationDate = DateTime.UtcNow
        };

        await _studentRepository.AddAsync(student, cancellationToken);

        return MapToDto(student);
    }

    public async Task<StudentDto> UpdateAsync(Guid id, UpdateStudentDto dto, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, cancellationToken);

        if (student == null)
            throw new KeyNotFoundException($"Estudiante con ID {id} no encontrado.");

        // Validate unique email
        if (!await _studentRepository.IsEmailUniqueAsync(dto.Email, id, cancellationToken))
        {
            throw new InvalidOperationException($"El correo {dto.Email} ya está registrado.");
        }

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.Email = dto.Email;

        await _studentRepository.UpdateAsync(student, cancellationToken);

        var updatedStudent = await _studentRepository.GetByIdWithInscriptionsAsync(id, cancellationToken);
        return MapToDto(updatedStudent!);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, cancellationToken);

        if (student == null)
            throw new KeyNotFoundException($"Estudiante con ID {id} no encontrado.");

        await _studentRepository.DeleteAsync(student, cancellationToken);
    }

    public async Task<IEnumerable<ClassmateDto>> GetStudentClassmatesAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdWithInscriptionsAsync(studentId, cancellationToken);

        if (student == null)
            throw new KeyNotFoundException($"Estudiante con ID {studentId} no encontrado.");

        var classmates = new List<ClassmateDto>();

        foreach (var inscription in student.Inscriptions)
        {
            var subjectClassmates = await _inscriptionRepository.GetClassmatesAsync(
                studentId,
                inscription.SubjectId,
                cancellationToken);

            classmates.AddRange(subjectClassmates.Select(c => new ClassmateDto(
                c.Id,
                c.FirstName,
                c.LastName,
                inscription.SubjectId,
                inscription.Subject.Name
            )));
        }

        return classmates;
    }

    private static StudentDto MapToDto(Student student)
    {
        return new StudentDto(
            student.Id,
            student.FirstName,
            student.LastName,
            student.Email,
            student.RegistrationDate,
            student.TotalCredits,
            student.Inscriptions.Select(i => new InscriptionDto(
                i.Id,
                i.SubjectId,
                i.Subject.Name,
                i.Subject.Code,
                i.Subject.Credits,
                i.Subject.Teacher.FullName,
                i.InscriptionDate
            )).ToList()
        );
    }
}
