using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.Projects.Queries.GetProjectList;
public class GetProjectListDto : BaseDto
{
    public string? Name { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Budget { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Project, GetProjectListDto>();
        }
    }
}
