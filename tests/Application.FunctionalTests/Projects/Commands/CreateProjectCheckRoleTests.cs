using ProjectManagmentApp.Application.Common.Exceptions;
using ProjectManagmentApp.Application.Common.Security;
using ProjectManagmentApp.Application.Projects.Commands.CreateProject;
using ProjectManagmentApp.Application.Projects.Commands.DeleteProject;
using ProjectManagmentApp.Domain.Entities;

namespace ProjectManagmentApp.Application.FunctionalTests.Projects.Commands;

using static Testing;

public class CreateProjectCheckRoleTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new CreateProjectCommand()
        {
            Name = "Deny",
            Budget = 100,
            OwnedBy = "User",
        };

        command.GetType().Should().BeDecoratedWith<AuthorizeAttribute>();

        var action = () => SendAsync(command);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task ShouldDenyNonManager()
    {
        await RunAsDefaultUserAsync();

        var command = new CreateProjectCommand()
        {
            Name = "Deny",
            Budget = 100,
            OwnedBy = "User",
        };

        var action = () => SendAsync(command);

        await action.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldAllowManager()
    {
        await RunAsManagerAsync();

        var command = new CreateProjectCommand()
        {
            Name = "Deny",
            Budget = 100,
            OwnedBy = "User",
        };

        var action = () => SendAsync(command);

        await action.Should().NotThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldDelete()
    {
        await RunAsManagerAsync();

        var project = await SendAsync(new CreateProjectCommand
        {
            Name = "New List 1",
            Budget = 100,
            OwnedBy = "User",
        });

        await SendAsync(new DeleteProjectCommand(project.Id));

        var count = await CountAsync<Project>();

        count.Should().Be(0);
    }
}
