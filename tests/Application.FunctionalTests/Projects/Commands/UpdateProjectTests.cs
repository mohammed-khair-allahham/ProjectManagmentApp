using ProjectManagmentApp.Application.Common.Exceptions;
using ProjectManagmentApp.Application.Projects.Commands.CreateProject;
using ProjectManagmentApp.Application.Projects.Commands.UpdateProject;
using ProjectManagmentApp.Domain.Entities;

namespace ProjectManagmentApp.Application.FunctionalTests.Projects.Commands;

using static Testing;

public class UpdateProjectTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidProjectId()
    {
        var id = 99;
        var command = new UpdateProjectCommand(id)
        {
            Id = id,
            Name = "New Name",
            Budget = 12,
            OwnedBy = "User"
        };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        var project = await SendAsync(new CreateProjectCommand
        {
            Name = "New Name",
            Budget = 12,
            OwnedBy = "User"
        });

        await SendAsync(new CreateProjectCommand
        {
            Name = "New Name",
            Budget = 12,
            OwnedBy = "User"
        });

        var command = new UpdateProjectCommand(project.Id)
        {
            Id = project.Id,
            Name = "New Name",
            Budget = 12,
            OwnedBy = "User"
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title")))
                .And.Errors["Title"].Should().Contain("'Title' must be unique.");
    }

    [Test]
    public async Task ShouldUpdateProject()
    {
        var userId = await RunAsDefaultUserAsync();

        var project = await SendAsync(new CreateProjectCommand
        {
            Name = "New Name",
            Budget = 12,
            OwnedBy = "User"
        });

        var command = new UpdateProjectCommand(project.Id)
        {
            Id = project.Id,
            Name = "Updated Name",
            Budget = 12,
            OwnedBy = "User"
        };

        await SendAsync(command);

        var list = await FindAsync<Project>(project.Id);

        list.Should().NotBeNull();
        list!.Name.Should().Be(command.Name);
        list.LastModifiedBy.Should().NotBeNull();
        list.LastModifiedBy.Should().Be(userId);
        list.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
