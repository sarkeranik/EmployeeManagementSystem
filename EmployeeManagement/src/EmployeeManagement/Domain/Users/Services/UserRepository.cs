namespace EmployeeManagement.Domain.Users.Services;

using EmployeeManagement.Domain.Users;
using EmployeeManagement.Databases;
using EmployeeManagement.Services;

public interface IUserRepository : IGenericRepository<User>
{
}

public sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly EmployeesDbContext _dbContext;

    public UserRepository(EmployeesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
