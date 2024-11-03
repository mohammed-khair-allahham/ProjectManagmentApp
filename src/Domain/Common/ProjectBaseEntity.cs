namespace ProjectManagmentApp.Domain.Common;
public class ProjectAppBaseEntity : BaseAuditableEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
