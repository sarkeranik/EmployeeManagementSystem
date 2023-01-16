namespace EmployeeManagement.UnitTests.UnitTests.Domain.Projects.Features;

using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using EmployeeManagement.Domain.Projects;
using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.Domain.Projects.Mappings;
using EmployeeManagement.Domain.Projects.Features;
using EmployeeManagement.Domain.Projects.Services;
using MapsterMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetProjectListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IProjectRepository> _projectRepository;

    public GetProjectListTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
    }
    
    [Test]
    public async Task can_get_paged_list_of_project()
    {
        //Arrange
        var fakeProjectOne = FakeProject.Generate();
        var fakeProjectTwo = FakeProject.Generate();
        var fakeProjectThree = FakeProject.Generate();
        var project = new List<Project>();
        project.Add(fakeProjectOne);
        project.Add(fakeProjectTwo);
        project.Add(fakeProjectThree);
        var mockDbData = project.AsQueryable().BuildMock();
        
        var queryParameters = new ProjectParametersDto() { PageSize = 1, PageNumber = 2 };

        _projectRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetProjectList.Query(queryParameters);
        var handler = new GetProjectList.Handler(_projectRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_project_list_using_Name()
    {
        //Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto()
            .RuleFor(p => p.Name, _ => "alpha")
            .Generate());
        var fakeProjectTwo = FakeProject.Generate(new FakeProjectForCreationDto()
            .RuleFor(p => p.Name, _ => "bravo")
            .Generate());
        var queryParameters = new ProjectParametersDto() { Filters = $"Name == {fakeProjectTwo.Name}" };

        var projectList = new List<Project>() { fakeProjectOne, fakeProjectTwo };
        var mockDbData = projectList.AsQueryable().BuildMock();

        _projectRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetProjectList.Query(queryParameters);
        var handler = new GetProjectList.Handler(_projectRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeProjectTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_project_by_Name()
    {
        //Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto()
            .RuleFor(p => p.Name, _ => "alpha")
            .Generate());
        var fakeProjectTwo = FakeProject.Generate(new FakeProjectForCreationDto()
            .RuleFor(p => p.Name, _ => "bravo")
            .Generate());
        var queryParameters = new ProjectParametersDto() { SortOrder = "-Name" };

        var ProjectList = new List<Project>() { fakeProjectOne, fakeProjectTwo };
        var mockDbData = ProjectList.AsQueryable().BuildMock();

        _projectRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetProjectList.Query(queryParameters);
        var handler = new GetProjectList.Handler(_projectRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeProjectTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeProjectOne, options =>
                options.ExcludingMissingMembers());
    }
}