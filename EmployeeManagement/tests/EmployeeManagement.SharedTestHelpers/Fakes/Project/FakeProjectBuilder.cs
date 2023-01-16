namespace EmployeeManagement.SharedTestHelpers.Fakes.Project;

using EmployeeManagement.Domain.Projects;
using EmployeeManagement.Domain.Projects.Dtos;

public class FakeProjectBuilder
{
    private ProjectForCreationDto _creationData = new FakeProjectForCreationDto().Generate();

    public FakeProjectBuilder WithDto(ProjectForCreationDto dto)
    {
        _creationData = dto;
        return this;
    }
    
    public Project Build()
    {
        var result = Project.Create(_creationData);
        return result;
    }
}