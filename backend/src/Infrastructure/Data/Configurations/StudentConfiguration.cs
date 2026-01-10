using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(s => s.Email)
            .IsUnique();

        builder.Property(s => s.RegistrationDate)
            .IsRequired();

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(15);
        builder.Ignore(s => s.FullName);
        builder.Ignore(s => s.TotalCredits);
    }
}
