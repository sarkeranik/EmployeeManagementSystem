namespace EmployeeManagement.UnitTests.UnitTests.Domain.Projects;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.Domain.Projects;
using EmployeeManagement.Domain.Projects.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateProjectTests
{
    private readonly Faker _faker;

    public CreateProjectTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_project()
    {
        // Arrange
        var projectToCreate = new FakeProjectForCreationDto().Generate();
        
        // Act
        var fakeProject = Project.Create(projectToCreate);

        // Assert
        fakeProject.Name.Should().Be(projectToCreate.Name);
        fakeProject.Description.Should().Be(projectToCreate.Description);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeProject = FakeProject.Generate();

        // Assert
        fakeProject.DomainEvents.Count.Should().Be(1);
        fakeProject.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ProjectCreated));
    }
}