using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.Projects.Queries.GetProjectDetails;
public class ProjectDetailsDto : BaseProjectAppDto
{
    public string? OwnedBy { get; set; }
    public string? OwnedByName { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset LastModified { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Project, ProjectDetailsDto>();
        }
    }
}
