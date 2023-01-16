namespace EmployeeManagement.FunctionalTests.FunctionalTests.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.FunctionalTests.TestUtilities;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateItemTests : TestBase
{
    [Test]
    public async Task create_item_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeItem = new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id)
            .Generate();

        // Act
        var route = ApiRoutes.Items.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeItem);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}