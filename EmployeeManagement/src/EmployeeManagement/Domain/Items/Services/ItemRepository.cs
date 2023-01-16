namespace EmployeeManagement.Domain.Items.Services;

using EmployeeManagement.Domain.Items;
using EmployeeManagement.Databases;
using EmployeeManagement.Services;

public interface IItemRepository : IGenericRepository<Item>
{
}

public sealed class ItemRepository : GenericRepository<Item>, IItemRepository
{
    private readonly EmployeesDbContext _dbContext;

    public ItemRepository(EmployeesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
