namespace EmployeeManagement.Domain.Clients.Services;

using EmployeeManagement.Domain.Clients;
using EmployeeManagement.Databases;
using EmployeeManagement.Services;

public interface IClientRepository : IGenericRepository<Client>
{
}

public sealed class ClientRepository : GenericRepository<Client>, IClientRepository
{
    private readonly EmployeesDbContext _dbContext;

    public ClientRepository(EmployeesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
