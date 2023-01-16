namespace EmployeeManagement.IntegrationTests.FeatureTests.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.Domain.Employees.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.User;

public class DeleteEmployeeCommandTests : TestBase
{
    [Test]
    public async Task can_delete_employee_from_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id).Generate());
        await InsertAsync(fakeEmployeeOne);
        var employee = await ExecuteDbContextAsync(db => db.Employees
            .FirstOrDefaultAsync(e => e.Id == fakeEmployeeOne.Id));

        // Act
        var command = new DeleteEmployee.Command(employee.Id);
        await SendAsync(command);
        var employeeResponse = await ExecuteDbContextAsync(db => db.Employees.CountAsync(e => e.Id == employee.Id));

        // Assert
        employeeResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_employee_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteEmployee.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_employee_from_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id).Generate());
        await InsertAsync(fakeEmployeeOne);
        var employee = await ExecuteDbContextAsync(db => db.Employees
            .FirstOrDefaultAsync(e => e.Id == fakeEmployeeOne.Id));

        // Act
        var command = new DeleteEmployee.Command(employee.Id);
        await SendAsync(command);
        var deletedEmployee = await ExecuteDbContextAsync(db => db.Employees
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == employee.Id));

        // Assert
        deletedEmployee?.IsDeleted.Should().BeTrue();
    }
}