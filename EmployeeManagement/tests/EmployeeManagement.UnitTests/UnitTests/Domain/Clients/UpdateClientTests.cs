namespace EmployeeManagement.UnitTests.UnitTests.Domain.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.Domain.Clients;
using EmployeeManagement.Domain.Clients.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateClientTests
{
    private readonly Faker _faker;

    public UpdateClientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_client()
    {
        // Arrange
        var fakeClient = FakeClient.Generate();
        var updatedClient = new FakeClientForUpdateDto().Generate();
        
        // Act
        fakeClient.Update(updatedClient);

        // Assert
        fakeClient.Name.Should().Be(updatedClient.Name);
        fakeClient.Address.Should().Be(updatedClient.Address);
        fakeClient.ProjectId.Should().Be(updatedClient.ProjectId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeClient = FakeClient.Generate();
        var updatedClient = new FakeClientForUpdateDto().Generate();
        fakeClient.DomainEvents.Clear();
        
        // Act
        fakeClient.Update(updatedClient);

        // Assert
        fakeClient.DomainEvents.Count.Should().Be(1);
        fakeClient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ClientUpdated));
    }
}