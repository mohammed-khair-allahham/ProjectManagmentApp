using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;

namespace ProjectManagmentApp.Application.Projects.Queries.GetProjectDetails;

[Authorize(Roles = Roles.Manager)]
public record GetProjectDetailsQuery(int Id) : IRequest<ProjectDetailsDto>
{
}

public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectDetailsQueryHandler(IApplicationDbContext context,
                                         IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ProjectDetailsDto> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
    {
        var projectEntity = await _context.Projects.AsNoTracking()
           .Where(l => l.Id == request.Id)
           .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, projectEntity);

        var projectDto = _mapper.Map<ProjectDetailsDto>(projectEntity);

        return projectDto;
    }
}
