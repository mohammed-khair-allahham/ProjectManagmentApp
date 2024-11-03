using Microsoft.Extensions.Logging;
using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Domain.Enums;
using ProjectManagmentApp.Domain.Events;

namespace ProjectManagmentApp.Application.ProjectTasks.EventHandlers;
internal class ProjectTaskCompletedEventHandler : INotificationHandler<ProjectTaskCompletedEvent>
{
    private readonly ILogger<ProjectTaskCompletedEventHandler> _logger;
    private readonly IApplicationDbContext _context;

    public ProjectTaskCompletedEventHandler(ILogger<ProjectTaskCompletedEventHandler> logger,
                                            IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Handle(ProjectTaskCompletedEvent notification, CancellationToken cancellationToken)
    {
        var loggerText = "ProjectManagmentApp Domain Event: {DomainEvent}";
        _logger.LogInformation($"Started {loggerText}", notification.GetType().Name);
       
        var readOnlyTasksDbSet = _context.Tasks.AsNoTracking();
      
        var allTasksCompleted = await readOnlyTasksDbSet.AllAsync(t => t.Status == ProjectTaskStatus.Completed && t.Id != notification.Task.Id);
        if (allTasksCompleted)
        {
            var project = await _context.Projects.FindAsync(notification.Task.ProjectId, cancellationToken);
            if(project is not null)
            {
                project.Status = ProjectStatus.Completed;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation(loggerText + "Project {ProjectId} Completed", notification.GetType().Name, project.Id);
            }
        }
        _logger.LogInformation($"Finished {loggerText}", notification.GetType().Name);
    }
}
