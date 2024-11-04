using ProjectManagmentApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagmentApp.Domain.Common;

namespace ProjectManagmentApp.Infrastructure.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> b)
    {
        b.Property(t => t.Name).IsRequired().HasMaxLength(400);
        b.Property(t => t.Description).IsRequired(false).HasMaxLength(5000);
        b.Property(t => t.StartDate).IsRequired(false);
        b.Property(t => t.EndDate).IsRequired(false);
        b.Property(t => t.AssignedTo).IsRequired(false).HasMaxLength(256);
        b.Property(t => t.Priority).IsRequired();
        b.Property(t => t.Status).IsRequired();
        b.HasQueryFilter(t => t.EntityStatus == BaseAuditableEntityStatus.Active);
    }
}
