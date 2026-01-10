namespace StudentRegistration.Domain.Entities;

public class Teacher
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public string FullName => $"{FirstName} {LastName}";
}
