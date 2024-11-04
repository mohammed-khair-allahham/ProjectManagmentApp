using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagmentApp.Domain.Entities;

public class ProjectTask : ProjectAppBaseEntity
{
    // Properties
    public string? AssignedTo { get; set; }
    public PriorityLevel Priority { get; set; }
    public ProjectTaskStatus Status { get; set; }

    // Navigation
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    // Events
    [NotMapped]
    public bool Completed
    {
        get => Status == ProjectTaskStatus.Completed;
        set
        {
            if (value && Status != ProjectTaskStatus.Completed)
            {
                Status = ProjectTaskStatus.Completed;
                AddDomainEvent(new ProjectTaskCompletedEvent(this));
            }
        }
    }
}
