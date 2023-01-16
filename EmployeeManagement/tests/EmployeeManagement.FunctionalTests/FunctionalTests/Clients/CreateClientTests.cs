namespace EmployeeManagement.FunctionalTests.FunctionalTests.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.FunctionalTests.TestUtilities;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateClientTests : TestBase
{
    [Test]
    public async Task create_client_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeClient = new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id)
            .Generate();

        // Act
        var route = ApiRoutes.Clients.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeClient);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}