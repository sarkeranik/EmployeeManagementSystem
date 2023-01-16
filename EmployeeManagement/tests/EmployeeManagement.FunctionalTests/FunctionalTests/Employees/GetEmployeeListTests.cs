namespace EmployeeManagement.FunctionalTests.FunctionalTests.Employees;

using EmployeeManagement.SharedTestHelpers.Fakes.Employee;
using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetEmployeeListTests : TestBase
{
    [Test]
    public async Task get_employee_list_returns_success()
    {
        // Arrange
        

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.Employees.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}