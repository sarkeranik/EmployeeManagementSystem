namespace EmployeeManagement.SharedTestHelpers.Fakes.Employee;

using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Employees.Dtos;

public class FakeEmployeeBuilder
{
    private EmployeeForCreationDto _creationData = new FakeEmployeeForCreationDto().Generate();

    public FakeEmployeeBuilder WithDto(EmployeeForCreationDto dto)
    {
        _creationData = dto;
        return this;
    }
    
    public Employee Build()
    {
        var result = Employee.Create(_creationData);
        return result;
    }
}