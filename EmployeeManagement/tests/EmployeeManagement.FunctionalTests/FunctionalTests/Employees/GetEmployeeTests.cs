namespace EmployeeManagement.FunctionalTests.FunctionalTests.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.FunctionalTests.TestUtilities;
using EmployeeManagement.SharedTestHelpers.Fakes.User;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetEmployeeTests : TestBase
{
    [Test]
    public async Task get_employee_returns_success_when_entity_exists()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeEmployee = FakeEmployee.Generate(new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id).Generate());
        await InsertAsync(fakeEmployee);

        // Act
        var route = ApiRoutes.Employees.GetRecord(fakeEmployee.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}