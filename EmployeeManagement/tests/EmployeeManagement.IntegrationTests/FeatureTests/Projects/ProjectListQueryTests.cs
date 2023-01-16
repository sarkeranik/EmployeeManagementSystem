namespace EmployeeManagement.IntegrationTests.FeatureTests.Projects;

using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Projects.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class ProjectListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_project_list()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        var fakeProjectTwo = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        var queryParameters = new ProjectParametersDto();

        await InsertAsync(fakeProjectOne, fakeProjectTwo);

        // Act
        var query = new GetProjectList.Query(queryParameters);
        var projects = await SendAsync(query);

        // Assert
        projects.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}