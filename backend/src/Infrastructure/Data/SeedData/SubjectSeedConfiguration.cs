using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Data.SeedData;

public class SubjectSeedConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        // Teacher 1 (María González) - teaches 2 subjects
        builder.HasData(
            new Subject
            {
                Id = new Guid("a1111111-1111-1111-1111-111111111111"),
                Code = "MAT101",
                Name = "Matemáticas Avanzadas",
                Credits = 3,
                TeacherId = new Guid("11111111-1111-1111-1111-111111111111")
            },
            new Subject
            {
                Id = new Guid("a1111111-1111-1111-1111-111111111112"),
                Code = "FIS101",
                Name = "Física General",
                Credits = 3,
                TeacherId = new Guid("11111111-1111-1111-1111-111111111111")
            }
        );

        // Teacher 2 (Carlos Rodríguez) - teaches 2 subjects
        builder.HasData(
            new Subject
            {
                Id = new Guid("a2222222-2222-2222-2222-222222222221"),
                Code = "PROG101",
                Name = "Programación I",
                Credits = 3,
                TeacherId = new Guid("22222222-2222-2222-2222-222222222222")
            },
            new Subject
            {
                Id = new Guid("a2222222-2222-2222-2222-222222222222"),
                Code = "BD101",
                Name = "Bases de Datos",
                Credits = 3,
                TeacherId = new Guid("22222222-2222-2222-2222-222222222222")
            }
        );

        // Teacher 3 (Ana Martínez) - teaches 2 subjects
        builder.HasData(
            new Subject
            {
                Id = new Guid("a3333333-3333-3333-3333-333333333331"),
                Code = "HIST101",
                Name = "Historia Universal",
                Credits = 3,
                TeacherId = new Guid("33333333-3333-3333-3333-333333333333")
            },
            new Subject
            {
                Id = new Guid("a3333333-3333-3333-3333-333333333332"),
                Code = "LIT101",
                Name = "Literatura Contemporánea",
                Credits = 3,
                TeacherId = new Guid("33333333-3333-3333-3333-333333333333")
            }
        );

        // Teacher 4 (Pedro López) - teaches 2 subjects
        builder.HasData(
            new Subject
            {
                Id = new Guid("a4444444-4444-4444-4444-444444444441"),
                Code = "QUIM101",
                Name = "Química Orgánica",
                Credits = 3,
                TeacherId = new Guid("44444444-4444-4444-4444-444444444444")
            },
            new Subject
            {
                Id = new Guid("a4444444-4444-4444-4444-444444444442"),
                Code = "BIO101",
                Name = "Biología Molecular",
                Credits = 3,
                TeacherId = new Guid("44444444-4444-4444-4444-444444444444")
            }
        );

        // Teacher 5 (Laura Fernández) - teaches 2 subjects
        builder.HasData(
            new Subject
            {
                Id = new Guid("a5555555-5555-5555-5555-555555555551"),
                Code = "ING101",
                Name = "Inglés Técnico",
                Credits = 3,
                TeacherId = new Guid("55555555-5555-5555-5555-555555555555")
            },
            new Subject
            {
                Id = new Guid("a5555555-5555-5555-5555-555555555552"),
                Code = "ADMIN101",
                Name = "Administración de Empresas",
                Credits = 3,
                TeacherId = new Guid("55555555-5555-5555-5555-555555555555")
            }
        );
    }
}
