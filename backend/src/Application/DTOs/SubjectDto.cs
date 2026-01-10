namespace StudentRegistration.Application.DTOs;

public record SubjectDto(
    Guid Id,
    string Code,
    string Name,
    int Credits,
    TeacherDto Teacher
);

public record SubjectListDto(
    Guid Id,
    string Code,
    string Name,
    int Credits,
    Guid TeacherId,
    string TeacherName
);
