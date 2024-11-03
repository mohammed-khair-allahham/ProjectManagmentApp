using ProjectManagmentApp.Application.Common.Interfaces;

namespace ProjectManagmentApp.Application.Projects.Commands.CreateProject;
public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public CreateProjectCommandValidator(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(400)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(p => p.Description)
            .MaximumLength(5000);

        RuleFor(p => p.OwnedBy)
           .NotEmpty()
           .MustAsync(OwnedByExist)
           .WithMessage("'{PropertyName}' must exist.")
                .WithErrorCode("Exist");

        RuleFor(p => p.Budget)
           .NotEmpty()
           .GreaterThan(0);
    }

    public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
         => await _context.Projects
                        .AllAsync(l => l.Name != name, cancellationToken);

    public async Task<bool> OwnedByExist(string? ownedBy, CancellationToken cancellationToken)
        => await _identityService.ExistAsync(ownedBy);
}
