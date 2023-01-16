namespace EmployeeManagement.FunctionalTests.FunctionalTests.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetClientListTests : TestBase
{
    [Test]
    public async Task get_client_list_returns_success()
    {
        // Arrange
        

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.Clients.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}