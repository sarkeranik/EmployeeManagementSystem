namespace EmployeeManagement.SharedTestHelpers.Fakes.Employee;

using AutoBogus;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Employees.Dtos;

public sealed class FakeEmployee
{
    public static Employee Generate(EmployeeForCreationDto employeeForCreationDto)
    {
        return Employee.Create(employeeForCreationDto);
    }

    public static Employee Generate()
    {
        return Generate(new FakeEmployeeForCreationDto().Generate());
    }
}