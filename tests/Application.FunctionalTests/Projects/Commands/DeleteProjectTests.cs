using ProjectManagmentApp.Application.Projects.Commands.CreateProject;
using ProjectManagmentApp.Application.Projects.Commands.DeleteProject;
using ProjectManagmentApp.Domain.Entities;

namespace ProjectManagmentApp.Application.FunctionalTests.Projects.Commands;

using static Testing;

public class DeleteProjectTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidProjectId()
    {
        var command = new DeleteProjectCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteProject()
    {
        var project = await SendAsync(new CreateProjectCommand
        {
            Name = "New List",
            Budget = 100,
            OwnedBy = "User",
        });

        await SendAsync(new DeleteProjectCommand(project.Id));

        var list = await FindAsync<Project>(project.Id);

        list.Should().BeNull();
    }
}
