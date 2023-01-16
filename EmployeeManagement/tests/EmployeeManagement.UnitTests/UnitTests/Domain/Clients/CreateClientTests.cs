namespace EmployeeManagement.UnitTests.UnitTests.Domain.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.Domain.Clients;
using EmployeeManagement.Domain.Clients.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateClientTests
{
    private readonly Faker _faker;

    public CreateClientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_client()
    {
        // Arrange
        var clientToCreate = new FakeClientForCreationDto().Generate();
        
        // Act
        var fakeClient = Client.Create(clientToCreate);

        // Assert
        fakeClient.Name.Should().Be(clientToCreate.Name);
        fakeClient.Address.Should().Be(clientToCreate.Address);
        fakeClient.ProjectId.Should().Be(clientToCreate.ProjectId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeClient = FakeClient.Generate();

        // Assert
        fakeClient.DomainEvents.Count.Should().Be(1);
        fakeClient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ClientCreated));
    }
}