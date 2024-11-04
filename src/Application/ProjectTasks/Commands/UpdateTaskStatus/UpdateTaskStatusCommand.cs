using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.TodoItems.Commands.UpdateTodoItem;

[Authorize(Roles = $"{Roles.Manager},{Roles.Employee}")]
public record UpdateTaskStatusCommand(int Id) : IRequest
{
    public ProjectTaskStatus Status { get; init; }
}

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public UpdateTaskStatusCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tasks
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);
        // Managers can update tasks, while employees can only update tasks
        // assigned to them
        if (_user.Role == Roles.Employee && entity.AssignedTo != _user.Name)
        {
            throw new UnauthorizedAccessException();
        }

        entity.Status = request.Status;

        _context.Tasks.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
