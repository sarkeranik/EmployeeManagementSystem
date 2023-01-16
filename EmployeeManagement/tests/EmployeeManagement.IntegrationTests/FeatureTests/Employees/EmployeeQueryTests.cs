namespace EmployeeManagement.IntegrationTests.FeatureTests.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.Domain.Employees.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.User;

public class EmployeeQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_employee_with_accurate_props()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id).Generate());
        await InsertAsync(fakeEmployeeOne);

        // Act
        var query = new GetEmployee.Query(fakeEmployeeOne.Id);
        var employee = await SendAsync(query);

        // Assert
        employee.Name.Should().Be(fakeEmployeeOne.Name);
        employee.Salary.Should().Be(fakeEmployeeOne.Salary);
        employee.Designation.Should().Be(fakeEmployeeOne.Designation);
        employee.EmployeeId.Should().Be(fakeEmployeeOne.EmployeeId);
    }

    [Test]
    public async Task get_employee_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetEmployee.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}