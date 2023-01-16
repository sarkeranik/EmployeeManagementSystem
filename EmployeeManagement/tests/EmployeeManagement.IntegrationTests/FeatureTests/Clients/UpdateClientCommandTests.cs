namespace EmployeeManagement.IntegrationTests.FeatureTests.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.Domain.Clients.Dtos;
using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Clients.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class UpdateClientCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_client_in_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id).Generate());
        var updatedClientDto = new FakeClientForUpdateDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id).Generate();
        await InsertAsync(fakeClientOne);

        var client = await ExecuteDbContextAsync(db => db.Clients
            .FirstOrDefaultAsync(c => c.Id == fakeClientOne.Id));
        var id = client.Id;

        // Act
        var command = new UpdateClient.Command(id, updatedClientDto);
        await SendAsync(command);
        var updatedClient = await ExecuteDbContextAsync(db => db.Clients.FirstOrDefaultAsync(c => c.Id == id));

        // Assert
        updatedClient.Name.Should().Be(updatedClientDto.Name);
        updatedClient.Address.Should().Be(updatedClientDto.Address);
        updatedClient.ProjectId.Should().Be(updatedClientDto.ProjectId);
    }
}