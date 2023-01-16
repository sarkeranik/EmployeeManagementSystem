namespace EmployeeManagement.IntegrationTests.FeatureTests.Clients;

using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Clients.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class ClientListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_client_list()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        var fakeProjectTwo = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne, fakeProjectTwo);

        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id).Generate());
        var fakeClientTwo = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectTwo.Id).Generate());
        var queryParameters = new ClientParametersDto();

        await InsertAsync(fakeClientOne, fakeClientTwo);

        // Act
        var query = new GetClientList.Query(queryParameters);
        var clients = await SendAsync(query);

        // Assert
        clients.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}