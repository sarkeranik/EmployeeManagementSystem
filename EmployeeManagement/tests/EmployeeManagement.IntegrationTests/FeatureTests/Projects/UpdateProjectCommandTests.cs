namespace EmployeeManagement.IntegrationTests.FeatureTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.Domain.Projects.Dtos;
using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Projects.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class UpdateProjectCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_project_in_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        var updatedProjectDto = new FakeProjectForUpdateDto().Generate();
        await InsertAsync(fakeProjectOne);

        var project = await ExecuteDbContextAsync(db => db.Projects
            .FirstOrDefaultAsync(p => p.Id == fakeProjectOne.Id));
        var id = project.Id;

        // Act
        var command = new UpdateProject.Command(id, updatedProjectDto);
        await SendAsync(command);
        var updatedProject = await ExecuteDbContextAsync(db => db.Projects.FirstOrDefaultAsync(p => p.Id == id));

        // Assert
        updatedProject.Name.Should().Be(updatedProjectDto.Name);
        updatedProject.Description.Should().Be(updatedProjectDto.Description);
    }
}