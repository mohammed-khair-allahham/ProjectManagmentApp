using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Domain.Entities;

namespace ProjectManagmentApp.Application.Projects.Commands;
public class ProjectResponseDto : BaseProjectAppDto
{
    public string? OwnedBy { get; set; }
    public string? Status { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Project, ProjectResponseDto>();
        }
    }
}


