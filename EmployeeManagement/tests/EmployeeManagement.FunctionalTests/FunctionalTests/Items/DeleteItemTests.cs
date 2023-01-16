namespace EmployeeManagement.FunctionalTests.FunctionalTests.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.FunctionalTests.TestUtilities;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteItemTests : TestBase
{
    [Test]
    public async Task delete_item_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeItem = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id).Generate());
        await InsertAsync(fakeItem);

        // Act
        var route = ApiRoutes.Items.Delete(fakeItem.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}