using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Data.SeedData;

public class TeacherSeedConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasData(
            new Teacher
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                FirstName = "María",
                LastName = "González",
                Email = "maria.gonzalez@university.edu"
            },
            new Teacher
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                FirstName = "Carlos",
                LastName = "Rodríguez",
                Email = "carlos.rodriguez@university.edu"
            },
            new Teacher
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                FirstName = "Ana",
                LastName = "Martínez",
                Email = "ana.martinez@university.edu"
            },
            new Teacher
            {
                Id = new Guid("44444444-4444-4444-4444-444444444444"),
                FirstName = "Pedro",
                LastName = "López",
                Email = "pedro.lopez@university.edu"
            },
            new Teacher
            {
                Id = new Guid("55555555-5555-5555-5555-555555555555"),
                FirstName = "Laura",
                LastName = "Fernández",
                Email = "laura.fernandez@university.edu"
            }
        );
    }
}
