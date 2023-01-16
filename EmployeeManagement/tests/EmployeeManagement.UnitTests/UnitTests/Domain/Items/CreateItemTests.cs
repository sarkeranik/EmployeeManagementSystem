namespace EmployeeManagement.UnitTests.UnitTests.Domain.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.Domain.Items;
using EmployeeManagement.Domain.Items.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateItemTests
{
    private readonly Faker _faker;

    public CreateItemTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_item()
    {
        // Arrange
        var itemToCreate = new FakeItemForCreationDto().Generate();
        
        // Act
        var fakeItem = Item.Create(itemToCreate);

        // Assert
        fakeItem.Name.Should().Be(itemToCreate.Name);
        fakeItem.Description.Should().Be(itemToCreate.Description);
        fakeItem.ProjectId.Should().Be(itemToCreate.ProjectId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeItem = FakeItem.Generate();

        // Assert
        fakeItem.DomainEvents.Count.Should().Be(1);
        fakeItem.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ItemCreated));
    }
}