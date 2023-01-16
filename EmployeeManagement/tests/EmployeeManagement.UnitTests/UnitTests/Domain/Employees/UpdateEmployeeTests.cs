namespace EmployeeManagement.UnitTests.UnitTests.Domain.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.Domain.Employees;
using EmployeeManagement.Domain.Employees.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateEmployeeTests
{
    private readonly Faker _faker;

    public UpdateEmployeeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_employee()
    {
        // Arrange
        var fakeEmployee = FakeEmployee.Generate();
        var updatedEmployee = new FakeEmployeeForUpdateDto().Generate();
        
        // Act
        fakeEmployee.Update(updatedEmployee);

        // Assert
        fakeEmployee.Name.Should().Be(updatedEmployee.Name);
        fakeEmployee.Salary.Should().Be(updatedEmployee.Salary);
        fakeEmployee.Designation.Should().Be(updatedEmployee.Designation);
        fakeEmployee.EmployeeId.Should().Be(updatedEmployee.EmployeeId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeEmployee = FakeEmployee.Generate();
        var updatedEmployee = new FakeEmployeeForUpdateDto().Generate();
        fakeEmployee.DomainEvents.Clear();
        
        // Act
        fakeEmployee.Update(updatedEmployee);

        // Assert
        fakeEmployee.DomainEvents.Count.Should().Be(1);
        fakeEmployee.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(EmployeeUpdated));
    }
}