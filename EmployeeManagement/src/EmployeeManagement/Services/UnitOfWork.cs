namespace EmployeeManagement.Services;

using EmployeeManagement.Databases;

public interface IUnitOfWork : IEmployeeManagementService
{
    Task<int> CommitChanges(CancellationToken cancellationToken = default);
}

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly EmployeesDbContext _dbContext;

    public UnitOfWork(EmployeesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitChanges(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
