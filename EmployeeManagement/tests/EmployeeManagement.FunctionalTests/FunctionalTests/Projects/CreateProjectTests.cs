namespace EmployeeManagement.FunctionalTests.FunctionalTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateProjectTests : TestBase
{
    [Test]
    public async Task create_project_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeProject = new FakeProjectForCreationDto().Generate();

        // Act
        var route = ApiRoutes.Projects.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeProject);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}