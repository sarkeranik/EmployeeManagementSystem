namespace EmployeeManagement.FunctionalTests.FunctionalTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetProjectListTests : TestBase
{
    [Test]
    public async Task get_project_list_returns_success()
    {
        // Arrange
        

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.Projects.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}