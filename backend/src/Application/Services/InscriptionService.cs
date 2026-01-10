using StudentRegistration.Application.DTOs;
using StudentRegistration.Domain.Entities;
using StudentRegistration.Domain.Exceptions;
using StudentRegistration.Domain.Repositories;

namespace StudentRegistration.Application.Services;

public class InscriptionService : IInscriptionService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IInscriptionRepository _inscriptionRepository;

    public InscriptionService(
        IStudentRepository studentRepository,
        ISubjectRepository subjectRepository,
        IInscriptionRepository inscriptionRepository)
    {
        _studentRepository = studentRepository;
        _subjectRepository = subjectRepository;
        _inscriptionRepository = inscriptionRepository;
    }

    public async Task<StudentDto> InscribeStudentAsync(CreateInscriptionDto dto, CancellationToken cancellationToken = default)
    {
        // Get student with current inscriptions
        var student = await _studentRepository.GetByIdWithInscriptionsAsync(dto.StudentId, cancellationToken);
        if (student == null)
            throw new KeyNotFoundException($"Estudiante con ID {dto.StudentId} no encontrado.");

        // Validate: Maximum 3 subjects total (current + new)
        if (student.Inscriptions.Count + dto.SubjectIds.Count > 3)
            throw new MaxSubjectsExceededException();

        // Get subjects to inscribe
        var subjects = new List<Subject>();
        foreach (var subjectId in dto.SubjectIds)
        {
            var subject = await _subjectRepository.GetByIdWithTeacherAsync(subjectId, cancellationToken);
            if (subject == null)
                throw new KeyNotFoundException($"Asignatura con ID {subjectId} no encontrada.");

            // Check if already inscribed
            if (await _inscriptionRepository.IsStudentInscribedInSubjectAsync(dto.StudentId, subjectId, cancellationToken))
                throw new StudentAlreadyEnrolledException(subject.Name);

            subjects.Add(subject);
        }

        // Validate: No duplicate teachers
        var teacherIds = subjects.Select(s => s.TeacherId).ToList();
        if (teacherIds.Count != teacherIds.Distinct().Count())
            throw new DuplicateTeacherException();

        // Validate: Maximum 9 credits total (current + new)
        var newCredits = subjects.Sum(s => s.Credits);
        var totalCreditsAfter = student.TotalCredits + newCredits;
        if (totalCreditsAfter > 9)
            throw new MaxCreditsExceededException();

        // Create inscriptions
        foreach (var subject in subjects)
        {
            var inscription = new Inscription
            {
                Id = Guid.NewGuid(),
                StudentId = dto.StudentId,
                SubjectId = subject.Id,
                InscriptionDate = DateTime.UtcNow
            };

            await _inscriptionRepository.AddAsync(inscription, cancellationToken);
        }

        // Return updated student
        var updatedStudent = await _studentRepository.GetByIdWithInscriptionsAsync(dto.StudentId, cancellationToken);
        return MapToStudentDto(updatedStudent!);
    }

    public async Task UnscribeStudentAsync(Guid studentId, Guid subjectId, CancellationToken cancellationToken = default)
    {
        var inscriptions = await _inscriptionRepository.GetByStudentIdAsync(studentId, cancellationToken);
        var inscription = inscriptions.FirstOrDefault(i => i.SubjectId == subjectId);

        if (inscription == null)
            throw new KeyNotFoundException($"No se encontró la inscripción para el estudiante {studentId} y la asignatura {subjectId}.");

        await _inscriptionRepository.DeleteAsync(inscription, cancellationToken);
    }

    public async Task<IEnumerable<InscriptionDto>> GetStudentInscriptionsAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var inscriptions = await _inscriptionRepository.GetByStudentIdAsync(studentId, cancellationToken);

        return inscriptions.Select(i => new InscriptionDto(
            i.Id,
            i.SubjectId,
            i.Subject.Name,
            i.Subject.Code,
            i.Subject.Credits,
            i.Subject.Teacher.FullName,
            i.InscriptionDate
        ));
    }

    public async Task<IEnumerable<StudentListDto>> GetSubjectStudentsAsync(Guid subjectId, CancellationToken cancellationToken = default)
    {
        var inscriptions = await _inscriptionRepository.GetBySubjectIdAsync(subjectId, cancellationToken);

        return inscriptions.Select(i => new StudentListDto(
            i.Student.Id,
            i.Student.FirstName,
            i.Student.LastName,
            i.Student.Email,
            i.Student.TotalCredits
        ));
    }

    private static StudentDto MapToStudentDto(Student student)
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
