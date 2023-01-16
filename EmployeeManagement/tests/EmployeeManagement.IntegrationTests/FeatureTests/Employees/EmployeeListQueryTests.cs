namespace EmployeeManagement.IntegrationTests.FeatureTests.Employees;

using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Employees.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.User;

public class EmployeeListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_employee_list()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        var fakeUserTwo = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne, fakeUserTwo);

        var fakeEmployeeOne = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id).Generate());
        var fakeEmployeeTwo = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserTwo.Id).Generate());
        var queryParameters = new EmployeeParametersDto();

        await InsertAsync(fakeEmployeeOne, fakeEmployeeTwo);

        // Act
        var query = new GetEmployeeList.Query(queryParameters);
        var employees = await SendAsync(query);

        // Assert
        employees.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}