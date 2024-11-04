using ProjectManagmentApp.Application.Common.Exceptions;
using ProjectManagmentApp.Application.Projects.Commands.CreateProject;
using ProjectManagmentApp.Domain.Entities;

namespace ProjectManagmentApp.Application.FunctionalTests.Projects.Commands;

using static Testing;

public class CreateProjectTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateProjectCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueName()
    {
        await SendAsync(new CreateProjectCommand
        {
            Name = "Shopping",
            Budget = 100,
            OwnedBy = "User",
        });

        var command = new CreateProjectCommand
        {
            Name = "Shopping",
            Budget = 100,
            OwnedBy = "User",
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateProject()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateProjectCommand
        {
            Name = "Tasks",
            Budget = 100,
            OwnedBy = "User",
        };

        var id = await SendAsync(command);

        var list = await FindAsync<Project>(id);

        list.Should().NotBeNull();
        list!.Name.Should().Be(command.Name);
        list.CreatedBy.Should().Be(userId);
        list.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
