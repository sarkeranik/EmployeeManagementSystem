namespace EmployeeManagement.IntegrationTests.FeatureTests.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.Domain.Employees.Dtos;
using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Employees.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.User;

public class UpdateEmployeeCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_employee_in_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id).Generate());
        var updatedEmployeeDto = new FakeEmployeeForUpdateDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id).Generate();
        await InsertAsync(fakeEmployeeOne);

        var employee = await ExecuteDbContextAsync(db => db.Employees
            .FirstOrDefaultAsync(e => e.Id == fakeEmployeeOne.Id));
        var id = employee.Id;

        // Act
        var command = new UpdateEmployee.Command(id, updatedEmployeeDto);
        await SendAsync(command);
        var updatedEmployee = await ExecuteDbContextAsync(db => db.Employees.FirstOrDefaultAsync(e => e.Id == id));

        // Assert
        updatedEmployee.Name.Should().Be(updatedEmployeeDto.Name);
        updatedEmployee.Salary.Should().Be(updatedEmployeeDto.Salary);
        updatedEmployee.Designation.Should().Be(updatedEmployeeDto.Designation);
        updatedEmployee.EmployeeId.Should().Be(updatedEmployeeDto.EmployeeId);
    }
}