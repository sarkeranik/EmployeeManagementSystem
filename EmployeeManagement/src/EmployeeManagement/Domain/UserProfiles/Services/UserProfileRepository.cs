namespace EmployeeManagement.Domain.UserProfiles.Services;

using EmployeeManagement.Domain.UserProfiles;
using EmployeeManagement.Databases;
using EmployeeManagement.Services;

public interface IUserProfileRepository : IGenericRepository<UserProfile>
{
}

public sealed class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
{
    private readonly EmployeesDbContext _dbContext;

    public UserProfileRepository(EmployeesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
