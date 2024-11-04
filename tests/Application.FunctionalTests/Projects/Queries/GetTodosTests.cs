using ProjectManagmentApp.Application.Projects.Queries.GetProjectList;
using ProjectManagmentApp.Domain.Entities;
using ProjectManagmentApp.Domain.ValueObjects;

namespace ProjectManagmentApp.Application.FunctionalTests.TodoLists.Queries;

using static Testing;

public class GetProjectsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        await RunAsDefaultUserAsync();

        await AddAsync(new Project
        {
            Name = "Shopping",
            Budget = 12,
            OwnedBy = "User",
            Tasks =
                    {
                        new ProjectTask { Name = "Apples" },
                        new ProjectTask { Name = "Milk"},
                        new ProjectTask { Name = "Bread"},
                        new ProjectTask { Name = "Toilet paper" },
                        new ProjectTask { Name = "Pasta" },
                        new ProjectTask { Name = "Tissues" },
                        new ProjectTask { Name = "Tuna" }
                    }
        });

        var query = new GetProjectListWithPaginationQuery();

        var result = await SendAsync(query);

        result.Items.Should().HaveCount(1);
    }

    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var query = new GetProjectListWithPaginationQuery();

        var action = () => SendAsync(query);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
