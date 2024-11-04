using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Mappings;
using ProjectManagmentApp.Application.Common.Models;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskList;

[Authorize]
public record GetTaskListWithPaginationQuery : IRequest<PaginatedList<GetTaskListDto>>
{
    // Create an endpoint that returns overdue tasks (tasks whose EndDate is in the past
    //but are not completed).
    public int? ProjectId { get; set; }
    public bool? OverDue { get; init; }
    public ProjectTaskStatus? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTaskListWithPaginationQueryHandler : IRequestHandler<GetTaskListWithPaginationQuery, PaginatedList<GetTaskListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;
    private readonly TimeProvider _time;

    public GetTaskListWithPaginationQueryHandler(IApplicationDbContext context,
                                                 IMapper mapper,
                                                 IUser user,
                                                 TimeProvider time)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
        _time = time;
    }
    public async Task<PaginatedList<GetTaskListDto>> Handle(GetTaskListWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tasks.AsNoTracking();

        if (request.ProjectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == request.ProjectId);
        }

        if (_user.Role == Roles.Employee)
        {
            query = query.Where(t => t.AssignedTo == _user.Name);
        }

        if (request.OverDue.HasValue)
        {
            query = query.Where(t => t.Created < _time.GetUtcNow());
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
