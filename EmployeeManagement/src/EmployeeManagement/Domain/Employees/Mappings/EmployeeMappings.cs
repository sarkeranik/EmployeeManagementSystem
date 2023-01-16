namespace EmployeeManagement.Domain.Employees.Mappings;

using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.Domain.Employees;
using Mapster;

public sealed class EmployeeMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Employee, EmployeeDto>();
        config.NewConfig<EmployeeForCreationDto, Employee>()
            .TwoWays();
        config.NewConfig<EmployeeForUpdateDto, Employee>()
            .TwoWays();
    }
}