using ProjectManagmentApp.Domain.Entities;

namespace ProjectManagmentApp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Project> Projects { get; }
    DbSet<ProjectTask> Tasks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
