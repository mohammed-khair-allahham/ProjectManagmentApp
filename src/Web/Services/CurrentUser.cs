using System.Security.Claims;

using ProjectManagmentApp.Application.Common.Interfaces;
using ProjectManagmentApp.Domain.Constants;

namespace ProjectManagmentApp.Web.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? Name => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    public string? Role => _httpContextAccessor.HttpContext?.User switch
    {
        var user when user is not null && user.IsInRole(Roles.Manager) => Roles.Manager,
        var user when user is not null && user.IsInRole(Roles.Employee) => Roles.Employee,
        _ => null // default case if no match is found
    };
}
