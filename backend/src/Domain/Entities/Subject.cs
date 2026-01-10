namespace StudentRegistration.Domain.Entities;

public class Subject
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Credits { get; set; } = 3; // Always 3 credits per subject
    public Guid TeacherId { get; set; }

    public Teacher Teacher { get; set; } = null!;
    public ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
}
