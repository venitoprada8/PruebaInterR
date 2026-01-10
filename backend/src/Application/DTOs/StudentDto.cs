namespace StudentRegistration.Application.DTOs;

public record StudentDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime RegistrationDate,
    int TotalCredits,
    List<InscriptionDto> Inscriptions
);

public record CreateStudentDto(
    string FirstName,
    string LastName,
    string Email
);

public record UpdateStudentDto(
    string FirstName,
    string LastName,
    string Email
);

public record StudentListDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    int TotalCredits
);
