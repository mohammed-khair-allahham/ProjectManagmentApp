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

    // Private
    private bool _completed;

    // Events
    [NotMapped]
    public bool Completed
    {
        get => _completed;
        set
        {
            if (value && !_completed)
            {
                AddDomainEvent(new ProjectTaskCompletedEvent(this));
            }

            _completed = value;
        }
    }
}
