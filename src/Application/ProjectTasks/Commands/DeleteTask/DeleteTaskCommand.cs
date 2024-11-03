using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Common;
using ProjectManagmentApp.Domain.Constants;

namespace ProjectManagmentApp.Application.ProjectTasks.Commands.DeleteTask;

[Authorize(Roles = $"{Roles.Administrator},{Roles.Manager}")]
[Authorize(Policy = Policies.CanDelete)]
public record DeleteTaskCommand(int Id) : IRequest;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tasks
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.EntityStatus = BaseAuditableEntityStatus.Deleted;

        _context.Tasks.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
