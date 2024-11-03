using System.Reflection;
using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagmentApp.Domain.Common;

namespace ProjectManagmentApp.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Project>(b =>
        {
            b.Property(p => p.Name).IsRequired().HasMaxLength(400);
            b.Property(p => p.Description).IsRequired(false).HasMaxLength(5000);
            b.Property(p => p.StartDate).IsRequired(false);
            b.Property(p => p.EndDate).IsRequired(false);
            b.Property(p => p.OwnedBy).IsRequired().HasMaxLength(36);
            b.Property(p => p.Budget).IsRequired().HasColumnType("decimal(18,4)");
            b.Property(p => p.Status).IsRequired();
            b.HasQueryFilter(p => p.EntityStatus == BaseAuditableEntityStatus.Active);
        });

        builder.Entity<ProjectTask>(b =>
        {
            b.Property(t => t.Name).IsRequired().HasMaxLength(400);
            b.Property(t => t.Description).IsRequired(false).HasMaxLength(5000);
            b.Property(t => t.StartDate).IsRequired(false);
            b.Property(t => t.EndDate).IsRequired(false);
            b.Property(t => t.AssignedTo).IsRequired(false).HasMaxLength(36);
            b.Property(t => t.Priority).IsRequired();
            b.Property(t => t.Status).IsRequired();
            b.HasQueryFilter(t => t.EntityStatus == BaseAuditableEntityStatus.Active);
        });
    }
}
