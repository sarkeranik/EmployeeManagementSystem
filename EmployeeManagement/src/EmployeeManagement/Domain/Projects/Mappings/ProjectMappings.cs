namespace EmployeeManagement.Domain.Projects.Mappings;

using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.Domain.Projects;
using Mapster;

public sealed class ProjectMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Project, ProjectDto>();
        config.NewConfig<ProjectForCreationDto, Project>()
            .TwoWays();
        config.NewConfig<ProjectForUpdateDto, Project>()
            .TwoWays();
    }
}