namespace EmployeeManagement.IntegrationTests.FeatureTests.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Employees.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using EmployeeManagement.SharedTestHelpers.Fakes.User;

public class AddEmployeeCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_employee_to_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeEmployeeOne = new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id).Generate();

        // Act
        var command = new AddEmployee.Command(fakeEmployeeOne);
        var employeeReturned = await SendAsync(command);
        var employeeCreated = await ExecuteDbContextAsync(db => db.Employees
            .FirstOrDefaultAsync(e => e.Id == employeeReturned.Id));

        // Assert
        employeeReturned.Name.Should().Be(fakeEmployeeOne.Name);
        employeeReturned.Salary.Should().Be(fakeEmployeeOne.Salary);
        employeeReturned.Designation.Should().Be(fakeEmployeeOne.Designation);
        employeeReturned.EmployeeId.Should().Be(fakeEmployeeOne.EmployeeId);

        employeeCreated.Name.Should().Be(fakeEmployeeOne.Name);
        employeeCreated.Salary.Should().Be(fakeEmployeeOne.Salary);
        employeeCreated.Designation.Should().Be(fakeEmployeeOne.Designation);
        employeeCreated.EmployeeId.Should().Be(fakeEmployeeOne.EmployeeId);
    }
}