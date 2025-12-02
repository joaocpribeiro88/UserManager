using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManager.Domain.Entities;

namespace UserManager.Infrastructure.Configurations;
public class EmploymentConfiguration : IEntityTypeConfiguration<Employment>
{
    public void Configure(EntityTypeBuilder<Employment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Company).IsRequired().HasMaxLength(150);
        builder.Property(e => e.MonthsOfExperience).IsRequired();
        builder.Property(e => e.Salary).IsRequired();
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired(false);
    }
}
