namespace EmployeeManagement.FunctionalTests.FunctionalTests.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.FunctionalTests.TestUtilities;
using EmployeeManagement.SharedTestHelpers.Fakes.User;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateEmployeeTests : TestBase
{
    [Test]
    public async Task create_employee_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var fakeEmployee = new FakeEmployeeForCreationDto()
            .RuleFor(e => e.EmployeeId, _ => fakeUserOne.Id)
            .Generate();

        // Act
        var route = ApiRoutes.Employees.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeEmployee);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}