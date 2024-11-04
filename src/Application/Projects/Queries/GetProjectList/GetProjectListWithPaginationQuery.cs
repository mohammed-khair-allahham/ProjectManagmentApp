using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Mappings;
using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.Projects.Queries.GetProjectList;

[Authorize(Roles = Roles.Manager)]
public record GetProjectListWithPaginationQuery() : IRequest<PaginatedList<GetProjectListDto>>
{
    public ProjectStatus? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProjectListQueryHandler : IRequestHandler<GetProjectListWithPaginationQuery, PaginatedList<GetProjectListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public GetProjectListQueryHandler(IApplicationDbContext context,
                                         IIdentityService identityService,
                                         IMapper mapper)
    {
        _context = context;
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetProjectListDto>> Handle(GetProjectListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Projects.AsNoTracking();

        if (request.Status.HasValue)
        {
            query = query.Where(p => p.Status == request.Status);
        }

        return await query.OrderBy(x => x.Name)
                    .ProjectTo<GetProjectListDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
