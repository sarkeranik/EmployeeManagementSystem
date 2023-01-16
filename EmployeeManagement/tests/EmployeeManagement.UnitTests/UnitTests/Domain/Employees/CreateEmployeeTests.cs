namespace EmployeeManagement.UnitTests.UnitTests.Domain.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Employees.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateEmployeeTests
{
    private readonly Faker _faker;

    public CreateEmployeeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_employee()
    {
        // Arrange
        var employeeToCreate = new FakeEmployeeForCreationDto().Generate();
        
        // Act
        var fakeEmployee = Employee.Create(employeeToCreate);

        // Assert
        fakeEmployee.Name.Should().Be(employeeToCreate.Name);
        fakeEmployee.Salary.Should().Be(employeeToCreate.Salary);
        fakeEmployee.Designation.Should().Be(employeeToCreate.Designation);
        fakeEmployee.EmployeeId.Should().Be(employeeToCreate.EmployeeId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeEmployee = FakeEmployee.Generate();

        // Assert
        fakeEmployee.DomainEvents.Count.Should().Be(1);
        fakeEmployee.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(EmployeeCreated));
    }
}