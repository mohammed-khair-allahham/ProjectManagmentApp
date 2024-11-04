using ProjectManagmentApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagmentApp.Domain.Common;

namespace ProjectManagmentApp.Infrastructure.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> b)
    {
        b.Property(p => p.Name).IsRequired().HasMaxLength(400);
        b.Property(p => p.Description).IsRequired(false).HasMaxLength(5000);
        b.Property(p => p.StartDate).IsRequired(false);
        b.Property(p => p.EndDate).IsRequired(false);
        b.Property(p => p.OwnedBy).IsRequired().HasMaxLength(256);
        b.Property(p => p.Budget).IsRequired().HasColumnType("decimal(18,4)");
        b.Property(p => p.Status).IsRequired();
        b.HasQueryFilter(p => p.EntityStatus == BaseAuditableEntityStatus.Active);
    }
}
