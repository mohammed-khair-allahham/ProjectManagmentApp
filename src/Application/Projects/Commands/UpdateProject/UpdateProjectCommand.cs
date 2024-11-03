using Microsoft.Extensions.Logging;
using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.Projects.Commands.UpdateProject;

[Authorize(Roles = $"{Roles.Administrator},{Roles.Manager}")]
[Authorize(Policy = Policies.CanUpdate)]
public record UpdateProjectCommand(int Id) : IRequest
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public decimal Budget { get; init; }
    public string? OwnedBy { get; init; }
    public ProjectStatus Status { get; init; }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(IApplicationDbContext context,
                                       IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.Budget = request.Budget;
        entity.OwnedBy = request.OwnedBy;
        entity.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
