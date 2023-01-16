namespace EmployeeManagement.IntegrationTests.FeatureTests.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.Domain.Clients.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class DeleteClientCommandTests : TestBase
{
    [Test]
    public async Task can_delete_client_from_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id).Generate());
        await InsertAsync(fakeClientOne);
        var client = await ExecuteDbContextAsync(db => db.Clients
            .FirstOrDefaultAsync(c => c.Id == fakeClientOne.Id));

        // Act
        var command = new DeleteClient.Command(client.Id);
        await SendAsync(command);
        var clientResponse = await ExecuteDbContextAsync(db => db.Clients.CountAsync(c => c.Id == client.Id));

        // Assert
        clientResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_client_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteClient.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_client_from_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id).Generate());
        await InsertAsync(fakeClientOne);
        var client = await ExecuteDbContextAsync(db => db.Clients
            .FirstOrDefaultAsync(c => c.Id == fakeClientOne.Id));

        // Act
        var command = new DeleteClient.Command(client.Id);
        await SendAsync(command);
        var deletedClient = await ExecuteDbContextAsync(db => db.Clients
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == client.Id));

        // Assert
        deletedClient?.IsDeleted.Should().BeTrue();
    }
}