namespace EmployeeManagement.IntegrationTests.FeatureTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Projects.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddProjectCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_project_to_db()
    {
        // Arrange
        var fakeProjectOne = new FakeProjectForCreationDto().Generate();

        // Act
        var command = new AddProject.Command(fakeProjectOne);
        var projectReturned = await SendAsync(command);
        var projectCreated = await ExecuteDbContextAsync(db => db.Projects
            .FirstOrDefaultAsync(p => p.Id == projectReturned.Id));

        // Assert
        projectReturned.Name.Should().Be(fakeProjectOne.Name);
        projectReturned.Description.Should().Be(fakeProjectOne.Description);

        projectCreated.Name.Should().Be(fakeProjectOne.Name);
        projectCreated.Description.Should().Be(fakeProjectOne.Description);
    }
}