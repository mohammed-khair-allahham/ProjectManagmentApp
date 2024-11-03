namespace ProjectManagmentApp.Domain.Events;
public class ProjectTaskCompletedEvent : BaseEvent
{
    public ProjectTaskCompletedEvent(ProjectTask task)
    {
        Task = task;
    }

    public ProjectTask Task { get; }
}
