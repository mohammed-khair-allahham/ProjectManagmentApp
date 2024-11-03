namespace ProjectManagmentApp.Web.Endpoints;

public class ProjectTasks : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
           .RequireAuthorization();
    }
}
