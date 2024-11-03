using System.Reflection;
using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        builder.Entity<Project>(p =>
        {
            p.Property(b => b.Name).IsRequired().HasMaxLength(400);
            p.Property(b => b.Description).IsRequired(false).HasMaxLength(5000);
            p.Property(b => b.StartDate).IsRequired(false);
            p.Property(b => b.EndDate).IsRequired(false);
            p.Property(b => b.OwnedBy).IsRequired().HasMaxLength(36);
            p.Property(b => b.Budget).IsRequired().HasColumnType("decimal(18,4)");
            p.Property(b => b.Status).IsRequired();
        });

        builder.Entity<ProjectTask>(t =>
        {
            t.Property(b => b.Name).IsRequired().HasMaxLength(400);
            t.Property(b => b.Description).IsRequired(false).HasMaxLength(5000);
            t.Property(b => b.StartDate).IsRequired(false);
            t.Property(b => b.EndDate).IsRequired(false);
            t.Property(b => b.AssignedTo).IsRequired(false).HasMaxLength(36);
            t.Property(b => b.Priority).IsRequired();
            t.Property(b => b.Status).IsRequired();
        });
    }
}
