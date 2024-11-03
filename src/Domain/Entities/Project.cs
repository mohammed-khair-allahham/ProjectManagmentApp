namespace ProjectManagmentApp.Domain.Entities;

public class Project : ProjectAppBaseEntity
{
    // Properties
    public decimal Budget { get; set; }
    public string? OwnedBy { get; set; }
    public ProjectStatus Status { get; set; }

    // Navigation property
    public IList<ProjectTask> Tasks { get; private set; } = new List<ProjectTask>();
}
