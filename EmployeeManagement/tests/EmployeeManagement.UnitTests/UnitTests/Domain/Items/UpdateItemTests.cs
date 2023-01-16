namespace EmployeeManagement.UnitTests.UnitTests.Domain.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.Domain.Items;
using EmployeeManagement.Domain.Items.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateItemTests
{
    private readonly Faker _faker;

    public UpdateItemTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_item()
    {
        // Arrange
        var fakeItem = FakeItem.Generate();
        var updatedItem = new FakeItemForUpdateDto().Generate();
        
        // Act
        fakeItem.Update(updatedItem);

        // Assert
        fakeItem.Name.Should().Be(updatedItem.Name);
        fakeItem.Description.Should().Be(updatedItem.Description);
        fakeItem.ProjectId.Should().Be(updatedItem.ProjectId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeItem = FakeItem.Generate();
        var updatedItem = new FakeItemForUpdateDto().Generate();
        fakeItem.DomainEvents.Clear();
        
        // Act
        fakeItem.Update(updatedItem);

        // Assert
        fakeItem.DomainEvents.Count.Should().Be(1);
        fakeItem.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(ItemUpdated));
    }
}