namespace EmployeeManagement.FunctionalTests.FunctionalTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class UpdateProjectRecordTests : TestBase
{
    [Test]
    public async Task put_project_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeProject = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        var updatedProjectDto = new FakeProjectForUpdateDto().Generate();
        await InsertAsync(fakeProject);

        // Act
        var route = ApiRoutes.Projects.Put(fakeProject.Id);
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedProjectDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}