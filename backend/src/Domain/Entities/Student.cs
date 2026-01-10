namespace StudentRegistration.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    
    public ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();

    public string FullName => $"{FirstName} {LastName}";
    public int TotalCredits => Inscriptions.Count * 3; // Numero de inscripciones × 3 créditos por asignatura
}
