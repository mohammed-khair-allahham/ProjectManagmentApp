using ProjectManagmentApp.Application.Common.Interfaces;

namespace ProjectManagmentApp.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UpdateTaskCommandValidator(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;

        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(400)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(p => p.Description)
            .MaximumLength(5000);

        RuleFor(p => p.AssignedTo)
           .NotEmpty()
           .MustAsync(AssignedToExist)
           .WithMessage("'{PropertyName}' must exist.")
                .WithErrorCode("Exist");

        RuleFor(p => p.ProjectId)
              .GreaterThan(0)
              .MustAsync(ProjectExist)
                 .WithMessage("'{PropertyName}' must exist.")
                 .WithErrorCode("Exist");
    }

    public async Task<bool> AssignedToExist(string? assignedTo, CancellationToken cancellationToken)
        => await _identityService.ExistAsync(assignedTo);

    public async Task<bool> ProjectExist(int id, CancellationToken cancellationToken)
        => await _context.Projects.AsNoTracking().AnyAsync(p => p.Id == id);
}
