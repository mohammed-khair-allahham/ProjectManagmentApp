using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Domain.Common;

namespace ProjectManagmentApp.Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(int Id) : IRequest;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var projectEntity = await _context.Projects.Include(p => p.Tasks)
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, projectEntity);

        projectEntity.EntityStatus = BaseAuditableEntityStatus.Deleted;
        foreach (var task in projectEntity.Tasks)
        {
            task.EntityStatus = BaseAuditableEntityStatus.Deleted;
        }

        _context.Projects.Update(projectEntity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
