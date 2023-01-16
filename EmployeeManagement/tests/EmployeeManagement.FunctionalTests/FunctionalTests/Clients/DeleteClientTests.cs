namespace EmployeeManagement.FunctionalTests.FunctionalTests.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.FunctionalTests.TestUtilities;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteClientTests : TestBase
{
    [Test]
    public async Task delete_client_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeClient = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id).Generate());
        await InsertAsync(fakeClient);

        // Act
        var route = ApiRoutes.Clients.Delete(fakeClient.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}