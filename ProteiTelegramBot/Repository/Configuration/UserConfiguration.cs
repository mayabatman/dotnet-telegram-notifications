using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Repository.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(x => x.ProteiLogin).IsRequired();
        builder.Property(x => x.TelegramLogin).IsRequired();

        builder
            .HasIndex(account => account.ProteiLogin)
            .IsUnique();

        builder
            .HasIndex(account => account.TelegramLogin)
            .IsUnique();
    }
}

public class DutyInformationConfiguration : IEntityTypeConfiguration<DutyInformation>
{
    public void Configure(EntityTypeBuilder<DutyInformation> builder)
    {
        builder.HasOne(s => s.DutyEmployee)
            .WithMany(g => g.DutyRoster)
            .HasForeignKey(s => s.DutyEmployeeId);

        builder
            .HasIndex(account => account.DutyDate)
            .IsUnique();
    }
}