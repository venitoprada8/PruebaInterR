namespace StudentRegistration.Application.DTOs;

public record InscriptionDto(
    Guid Id,
    Guid SubjectId,
    string SubjectName,
    string SubjectCode,
    int Credits,
    string TeacherName,
    DateTime InscriptionDate
);

public record CreateInscriptionDto(
    Guid StudentId,
    List<Guid> SubjectIds
);

public record ClassmateDto(
    Guid StudentId,
    string FirstName,
    string LastName,
    Guid SubjectId,
    string SubjectName
);
