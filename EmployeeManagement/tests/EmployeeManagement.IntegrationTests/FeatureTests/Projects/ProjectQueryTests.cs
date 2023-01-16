namespace EmployeeManagement.IntegrationTests.FeatureTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.Domain.Projects.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class ProjectQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_project_with_accurate_props()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        // Act
        var query = new GetProject.Query(fakeProjectOne.Id);
        var project = await SendAsync(query);

        // Assert
        project.Name.Should().Be(fakeProjectOne.Name);
        project.Description.Should().Be(fakeProjectOne.Description);
    }

    [Test]
    public async Task get_project_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetProject.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}