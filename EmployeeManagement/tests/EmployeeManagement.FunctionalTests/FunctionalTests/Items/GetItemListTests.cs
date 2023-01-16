namespace EmployeeManagement.FunctionalTests.FunctionalTests.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.FunctionalTests.TestUtilities;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetItemListTests : TestBase
{
    [Test]
    public async Task get_item_list_returns_success()
    {
        // Arrange
        

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.Items.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}