namespace EmployeeManagement.SharedTestHelpers.Fakes.Employee;

using AutoBogus;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Employees.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public sealed class FakeEmployeeForCreationDto : AutoFaker<EmployeeForCreationDto>
{
    public FakeEmployeeForCreationDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(e => e.ExampleIntProperty, e => e.Random.Number(50, 100000));
        //RuleFor(e => e.ExampleDateProperty, e => e.Date.Past());
    }
}