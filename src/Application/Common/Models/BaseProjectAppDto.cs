namespace ProjectManagmentApp.Application.Common.Models;

public class BaseProjectAppDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
