using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Domain.Entities;

namespace ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskDetails;
public class TaskDetailsDto : BaseProjectAppDto
{
    public string? AssignedTo { get; set; }
    public string? AssignedName { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }

    public int ProjectId { get; set; }
    public string? ProjectName { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProjectTask, TaskDetailsDto>();
        }
    }
}
