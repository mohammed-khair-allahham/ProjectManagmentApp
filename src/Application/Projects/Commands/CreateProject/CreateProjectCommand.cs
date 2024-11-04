using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.Projects.Commands.CreateProject;

[Authorize(Roles = Roles.Manager)]
public record CreateProjectCommand : IRequest<ProjectResponseDto>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public decimal Budget { get; init; }
    public string? OwnedBy { get; init; }
    public ProjectStatus Status { get; init; } = ProjectStatus.NotStarted;
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(IApplicationDbContext context,
                                       IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
  
    public async Task<ProjectResponseDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectEntity = new Project
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Budget = request.Budget,
            OwnedBy = request.OwnedBy,
            Status = request.Status
        };

        _context.Projects.Add(projectEntity);

        await _context.SaveChangesAsync(cancellationToken);

        var projectDto = _mapper.Map<ProjectResponseDto>(projectEntity);

        return projectDto;
    }
}
