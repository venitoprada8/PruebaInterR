using StudentRegistration.Application.DTOs;
using StudentRegistration.Domain.Repositories;

namespace StudentRegistration.Application.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly IInscriptionRepository _inscriptionRepository;

    public SubjectService(
        ISubjectRepository subjectRepository,
        IInscriptionRepository inscriptionRepository)
    {
        _subjectRepository = subjectRepository;
        _inscriptionRepository = inscriptionRepository;
    }

    public async Task<SubjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var subject = await _subjectRepository.GetByIdWithTeacherAsync(id, cancellationToken);

        if (subject == null)
            return null;

        return new SubjectDto(
            subject.Id,
            subject.Code,
            subject.Name,
            subject.Credits,
            new TeacherDto(
                subject.Teacher.Id,
                subject.Teacher.FirstName,
                subject.Teacher.LastName,
                subject.Teacher.Email
            )
        );
    }

    public async Task<IEnumerable<SubjectListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var subjects = await _subjectRepository.GetAllWithTeachersAsync(cancellationToken);

        return subjects.Select(s => new SubjectListDto(
            s.Id,
            s.Code,
            s.Name,
            s.Credits,
            s.TeacherId,
            s.Teacher.FullName
        ));
    }

    public async Task<IEnumerable<SubjectListDto>> GetAvailableSubjectsForStudentAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        // Get all subjects
        var allSubjects = await _subjectRepository.GetAllWithTeachersAsync(cancellationToken);

        // Get student's current inscriptions
        var inscriptions = await _inscriptionRepository.GetByStudentIdAsync(studentId, cancellationToken);
        var inscribedSubjectIds = inscriptions.Select(i => i.SubjectId).ToHashSet();
        var inscribedTeacherIds = inscriptions.Select(i => i.Subject.TeacherId).ToHashSet();

        // Calculate current student credits
        var currentCredits = inscriptions.Sum(i => i.Subject.Credits);
        const int MaxCredits = 9;
        const int MaxSubjects = 3;

        // Filter: exclude already inscribed subjects, subjects with same teachers,
        // and subjects that would exceed max credits or max subjects
        var availableSubjects = allSubjects
            .Where(s =>
                !inscribedSubjectIds.Contains(s.Id) && // Not already inscribed
                !inscribedTeacherIds.Contains(s.TeacherId) && // No same teacher
                inscriptions.Count() < MaxSubjects && // Not at max subjects
                currentCredits + s.Credits <= MaxCredits) // Won't exceed max credits
            .Select(s => new SubjectListDto(
                s.Id,
                s.Code,
                s.Name,
                s.Credits,
                s.TeacherId,
                s.Teacher.FullName
            ))
            .ToList();

        return availableSubjects;
    }
}
