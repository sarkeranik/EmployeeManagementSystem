namespace EmployeeManagement.FunctionalTests.FunctionalTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetProjectTests : TestBase
{
    [Test]
    public async Task get_project_returns_success_when_entity_exists()
    {
        // Arrange
        var fakeProject = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProject);

        // Act
        var route = ApiRoutes.Projects.GetRecord(fakeProject.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}