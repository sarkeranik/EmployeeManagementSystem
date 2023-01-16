namespace EmployeeManagement.Domain.Employees.Services;

using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Databases;
using EmployeeManagement.Services;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
}

public sealed class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    private readonly EmployeesDbContext _dbContext;

    public EmployeeRepository(EmployeesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
