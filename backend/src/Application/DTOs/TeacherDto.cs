namespace StudentRegistration.Application.DTOs;

public record TeacherDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);

public record TeacherWithSubjectsDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    List<SubjectListDto> Subjects
);
