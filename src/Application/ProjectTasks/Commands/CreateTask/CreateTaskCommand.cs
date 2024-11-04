using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Domain.Constants;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Domain.Enums;

namespace ProjectManagmentApp.Application.ProjectTasks.Commands.CreateTask;

[Authorize(Roles = Roles.Manager)]
public record CreateTaskCommand : IRequest<TaskResponseDto>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? AssignedTo { get; init; }
    public PriorityLevel Priority { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public int ProjectId { get; init; }
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly TimeProvider _time;

    public CreateTaskCommandHandler(IApplicationDbContext context,
                                    IMapper mapper,
                                    TimeProvider time)
    {
        _context = context;
        _mapper = mapper;
        _time = time;
    }

    public async Task<TaskResponseDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProjectTask
        {
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AssignedTo = request.AssignedTo,
            Priority = request.Priority,
            Status = request.Status,
            ProjectId = request.ProjectId
        };

        _context.Tasks.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        var taskDto = _mapper.Map<TaskResponseDto>(entity);

        return taskDto;
    }
}
