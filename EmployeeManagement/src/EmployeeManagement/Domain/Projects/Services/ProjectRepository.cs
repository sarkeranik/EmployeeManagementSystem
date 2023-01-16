namespace EmployeeManagement.Domain.Projects.Services;

using EmployeeManagement.Domain.Projects;
using EmployeeManagement.Databases;
using EmployeeManagement.Services;

public interface IProjectRepository : IGenericRepository<Project>
{
}

public sealed class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    private readonly EmployeesDbContext _dbContext;

    public ProjectRepository(EmployeesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
