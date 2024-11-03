using Microsoft.Extensions.Logging;
using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.Projects.Commands.CreateProject;

[Authorize(Roles = $"{Roles.Administrator},{Roles.Manager}")]
[Authorize(Policy = Policies.CanCreate)]
public record CreateProjectCommand : IRequest<ProjectResponseDto>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Budget { get; set; }
    public string? OwnedBy { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateProjectCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(IApplicationDbContext context,
                                       ILogger<CreateProjectCommandHandler> logger,
                                       IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }
  
    public async Task<ProjectResponseDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var loggerText = "ProjectManagmentApp Command: {Command}";
        _logger.LogInformation($"Started {loggerText}", request.GetType().Name);

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

        _logger.LogInformation($"Finished {loggerText}", request.GetType().Name);

        return projectDto;
    }
}
