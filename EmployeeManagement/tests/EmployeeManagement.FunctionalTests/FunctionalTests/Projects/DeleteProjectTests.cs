namespace EmployeeManagement.FunctionalTests.FunctionalTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteProjectTests : TestBase
{
    [Test]
    public async Task delete_project_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeProject = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProject);

        // Act
        var route = ApiRoutes.Projects.Delete(fakeProject.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}