namespace EmployeeManagement.FunctionalTests.FunctionalTests.HealthChecks;

using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class HealthCheckTests : TestBase
{
    [Test]
    public async Task health_check_returns_ok()
    {
        // Arrange
        // N/A

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.Health);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}