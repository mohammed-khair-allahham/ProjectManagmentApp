using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.ProjectTasks;
public class TaskResponseDto : BaseProjectAppDto
{
    public string? AssignedTo { get; set; }
    public PriorityLevel Priority { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public int ProjectId { get; set; }
    public string? ProjectName { get; set; }
}
