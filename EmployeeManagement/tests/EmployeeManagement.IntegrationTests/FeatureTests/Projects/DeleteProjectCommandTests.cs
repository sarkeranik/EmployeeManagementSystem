namespace EmployeeManagement.IntegrationTests.FeatureTests.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.Domain.Projects.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class DeleteProjectCommandTests : TestBase
{
    [Test]
    public async Task can_delete_project_from_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);
        var project = await ExecuteDbContextAsync(db => db.Projects
            .FirstOrDefaultAsync(p => p.Id == fakeProjectOne.Id));

        // Act
        var command = new DeleteProject.Command(project.Id);
        await SendAsync(command);
        var projectResponse = await ExecuteDbContextAsync(db => db.Projects.CountAsync(p => p.Id == project.Id));

        // Assert
        projectResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_project_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteProject.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_project_from_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);
        var project = await ExecuteDbContextAsync(db => db.Projects
            .FirstOrDefaultAsync(p => p.Id == fakeProjectOne.Id));

        // Act
        var command = new DeleteProject.Command(project.Id);
        await SendAsync(command);
        var deletedProject = await ExecuteDbContextAsync(db => db.Projects
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == project.Id));

        // Assert
        deletedProject?.IsDeleted.Should().BeTrue();
    }
}