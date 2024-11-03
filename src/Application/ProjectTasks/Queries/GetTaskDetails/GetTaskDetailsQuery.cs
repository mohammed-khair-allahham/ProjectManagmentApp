using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;

namespace ProjectManagmentApp.Application.ProjectTasks.Queries.GetTaskDetails;

[Authorize]
[Authorize(Policy = Policies.CanGet)]
public record GetTaskDetailsQuery(int Id) : IRequest<TaskDetailsDto>
{
}

public class GetTaskDetailsQueryHandler : IRequestHandler<GetTaskDetailsQuery, TaskDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public GetTaskDetailsQueryHandler(IApplicationDbContext context,
                                         IIdentityService identityService,
                                         IMapper mapper,
                                         IUser user)
    {
        _context = context;
        _identityService = identityService;
        _mapper = mapper;
        _user = user;
    }
    public async Task<TaskDetailsDto> Handle(GetTaskDetailsQuery request, CancellationToken cancellationToken)
    {
        var taskEntity = await _context.Tasks.AsNoTracking()
           .Include(t => t.Project)
           .Where(l => l.Id == request.Id)
           .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id, taskEntity);

        // Managers can update tasks, while employees can only update tasks
        // assigned to them
        if (_user.Role == Roles.Employee && taskEntity.AssignedTo != _user.Id)
        {
            throw new UnauthorizedAccessException();
        }

        var projectDto = _mapper.Map<TaskDetailsDto>(taskEntity);

        projectDto.ProjectName = taskEntity.Project.Name;
        if (projectDto.AssignedTo is not null)
        {
            var username = await _identityService.GetUserNameAsync(projectDto.AssignedTo);
            projectDto.AssignedName = username;
        }

        return projectDto;
    }
}
