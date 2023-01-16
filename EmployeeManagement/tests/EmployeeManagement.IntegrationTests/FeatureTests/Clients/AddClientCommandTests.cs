namespace EmployeeManagement.IntegrationTests.FeatureTests.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Clients.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class AddClientCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_client_to_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeClientOne = new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id).Generate();

        // Act
        var command = new AddClient.Command(fakeClientOne);
        var clientReturned = await SendAsync(command);
        var clientCreated = await ExecuteDbContextAsync(db => db.Clients
            .FirstOrDefaultAsync(c => c.Id == clientReturned.Id));

        // Assert
        clientReturned.Name.Should().Be(fakeClientOne.Name);
        clientReturned.Address.Should().Be(fakeClientOne.Address);
        clientReturned.ProjectId.Should().Be(fakeClientOne.ProjectId);

        clientCreated.Name.Should().Be(fakeClientOne.Name);
        clientCreated.Address.Should().Be(fakeClientOne.Address);
        clientCreated.ProjectId.Should().Be(fakeClientOne.ProjectId);
    }
}