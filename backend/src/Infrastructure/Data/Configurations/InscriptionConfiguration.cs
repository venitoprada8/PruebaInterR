using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Domain.Entities;

namespace StudentRegistration.Infrastructure.Data.Configurations;

public class InscriptionConfiguration : IEntityTypeConfiguration<Inscription>
{
    public void Configure(EntityTypeBuilder<Inscription> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InscriptionDate)
            .IsRequired();

        builder.HasOne(i => i.Student)
            .WithMany(s => s.Inscriptions)
            .HasForeignKey(i => i.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Subject)
            .WithMany(s => s.Inscriptions)
            .HasForeignKey(i => i.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ensure a student cannot inscribe in the same subject twice
        builder.HasIndex(i => new { i.StudentId, i.SubjectId })
            .IsUnique();
    }
}
