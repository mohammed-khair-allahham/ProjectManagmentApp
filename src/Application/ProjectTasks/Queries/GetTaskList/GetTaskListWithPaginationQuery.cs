using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Mappings;
using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskList;

[Authorize]
[Authorize(Policy = Policies.CanGet)]
public record GetTaskListWithPaginationQuery : IRequest<PaginatedList<GetTaskListDto>>
{
    public ProjectTaskStatus? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTaskListWithPaginationQueryHandler : IRequestHandler<GetTaskListWithPaginationQuery, PaginatedList<GetTaskListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public GetTaskListWithPaginationQueryHandler(IApplicationDbContext context,
                                                 IMapper mapper,
                                                 IUser user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }
    public async Task<PaginatedList<GetTaskListDto>> Handle(GetTaskListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tasks.AsNoTracking();

        if (_user.Role == Roles.Employee)
        {
            query = query.Where(t => t.AssignedTo == _user.Id);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(t => t.Status == request.Status);
        }

        return await query.OrderBy(x => x.Name)
                    .ProjectTo<GetTaskListDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
