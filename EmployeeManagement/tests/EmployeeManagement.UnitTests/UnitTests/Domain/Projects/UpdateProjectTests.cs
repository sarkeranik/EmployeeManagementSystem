namespace EmployeeManagement.UnitTests.UnitTests.Domain.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.Domain.Projects;
using EmployeeManagement.Domain.Projects.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateProjectTests
{
    private readonly Faker _faker;

    public UpdateProjectTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_project()
    {
        // Arrange
        var fakeProject = FakeProject.Generate();
        var updatedProject = new FakeProjectForUpdateDto().Generate();
        
        // Act
        fakeProject.Update(updatedProject);

        // Assert
        fakeProject.Name.Should().Be(updatedProject.Name);
        fakeProject.Description.Should().Be(updatedProject.Description);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeProject = FakeProject.Generate();
        var updatedProject = new FakeProjectForUpdateDto().Generate();
        fakeProject.DomainEvents.Clear();
        
        // Act
        fakeProject.Update(updatedProject);

        // Assert
        fakeProject.DomainEvents.Count.Should().Be(1);
        fakeProject.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ProjectUpdated));
    }
}