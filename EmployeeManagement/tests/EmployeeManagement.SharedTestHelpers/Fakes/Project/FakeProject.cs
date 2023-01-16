namespace EmployeeManagement.SharedTestHelpers.Fakes.Project;

using AutoBogus;
using EmployeeManagement.Domain.Projects;
using EmployeeManagement.Domain.Projects.Dtos;

public sealed class FakeProject
{
    public static Project Generate(ProjectForCreationDto projectForCreationDto)
    {
        return Project.Create(projectForCreationDto);
    }

    public static Project Generate()
    {
        return Generate(new FakeProjectForCreationDto().Generate());
    }
}