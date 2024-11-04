using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Domain.Entities;

namespace ProjectManagmentApp.Application.Projects.Queries.GetProjectList;
public class GetProjectListDto : BaseDto
{
    public string? Name { get; set; }
    public string? OwnedBy { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Budget { get; set; }
    public DateTimeOffset Created { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Project, GetProjectListDto>();
        }
    }
}
