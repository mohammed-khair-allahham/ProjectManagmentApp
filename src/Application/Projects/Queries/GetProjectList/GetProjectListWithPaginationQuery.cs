using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Mappings;
using ProjectManagmentApp.Application.Common.Models;

namespace ProjectManagmentApp.Application.Projects.Queries.GetProjectList;


public record GetProjectListWithPaginationQuery() : IRequest<PaginatedList<GetProjectListDto>>
{
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

    public Task<PaginatedList<GetProjectListDto>> Handle(GetProjectListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Projects.AsNoTracking();

        return query.OrderByDescending(x => x.Id)
                    .ProjectTo<GetProjectListDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
