using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskDetails;
public class TaskDetailsDto : BaseProjectAppDto
{
    public string? AssignedTo { get; set; }
    public string? AssignedName { get; set; }
    public PriorityLevel Priority { get; set; }
    public ProjectTaskStatus Status { get; set; }

    public int ProjectId { get; set; }
    public string? ProjectName { get; set; }
}
