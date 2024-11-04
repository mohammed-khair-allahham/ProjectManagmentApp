using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskList;
public class GetTaskListDto : BaseDto
{
    public string? Name { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset Created { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ProjectTask, GetTaskListDto>();
        }
    }
}
