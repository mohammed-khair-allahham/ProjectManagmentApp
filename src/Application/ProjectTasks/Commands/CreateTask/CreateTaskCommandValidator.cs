using ProjectManagmentApp.Application.Common.Interfaces;

namespace ProjectManagmentApp.Application.ProjectTasks.Commands.CreateTask;
public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public CreateTaskCommandValidator(IApplicationDbContext context, IIdentityService identityService)
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

        RuleFor(p => p.StartDate)
            .LessThan(p => p.EndDate);

        RuleFor(p => p)
              .MustAsync(StartAndEndDateMatch)
                 .WithMessage("'StartDate and EndDate' must be between the project's 'StartDate and EndDate'.")
                 .WithErrorCode("NotMatch");
    }

    public async Task<bool> AssignedToExist(string? assignedTo, CancellationToken cancellationToken)
        => await _identityService.ExistAsync(assignedTo);

    public async Task<bool> ProjectExist(int id, CancellationToken cancellationToken)
        => await _context.Projects.AsNoTracking().AnyAsync(p => p.Id == id);

    public async Task<bool> StartAndEndDateMatch(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        // Fetch project from the database
        var project = await _context.Projects.AsNoTracking()
                                             .SingleOrDefaultAsync(p => p.Id == command.ProjectId, cancellationToken);
        
        Guard.Against.Null(project);

        // Check StartDate constraints
        if (project.StartDate.HasValue && command.StartDate.HasValue)
        {
            // Task start date should not be before project start date
            if (command.StartDate < project.StartDate)
                return false;
        }

        // Check EndDate constraints
        if (project.EndDate.HasValue && command.EndDate.HasValue)
        {
            // Task end date should not be after project end date
            if (command.EndDate > project.EndDate)
                return false;
        }

        // If none of the constraints are violated, dates match
        return true;
    }
}
