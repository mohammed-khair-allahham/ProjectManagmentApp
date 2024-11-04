using ProjectManagmentApp.Application.Common.Interfaces;

namespace ProjectManagmentApp.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UpdateProjectCommandValidator(IApplicationDbContext context, IIdentityService identityService)
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
           .MaximumLength(256)
           .MustAsync(OwnedByExist)
           .WithMessage("'{PropertyName}' must exist.")
                .WithErrorCode("NotExist");

        RuleFor(p => p.Budget)
           .NotEmpty()
           .GreaterThan(0);
    }

    public async Task<bool> BeUniqueTitle(UpdateProjectCommand model, string title, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != title, cancellationToken);
    }

    public async Task<bool> OwnedByExist(string? ownedBy, CancellationToken cancellationToken)
        => await _identityService.ExistAsync(ownedBy);
}
