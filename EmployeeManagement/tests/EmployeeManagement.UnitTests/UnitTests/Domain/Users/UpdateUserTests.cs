namespace EmployeeManagement.UnitTests.UnitTests.Domain.Users;

using EmployeeManagement.SharedTestHelpers.Fakes.User;
using EmployeeManagement.Domain.Users;
using EmployeeManagement.Domain.Users.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateUserTests
{
    private readonly Faker _faker;

    public UpdateUserTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_user()
    {
        // Arrange
        var fakeUser = FakeUser.Generate();
        var updatedUser = new FakeUserForUpdateDto().Generate();
        
        // Act
        fakeUser.Update(updatedUser);

        // Assert
        fakeUser.Name.Should().Be(updatedUser.Name);
        fakeUser.UserProfileId.Should().Be(updatedUser.UserProfileId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeUser = FakeUser.Generate();
        var updatedUser = new FakeUserForUpdateDto().Generate();
        fakeUser.DomainEvents.Clear();
        
        // Act
        fakeUser.Update(updatedUser);

        // Assert
        fakeUser.DomainEvents.Count.Should().Be(1);
        fakeUser.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserUpdated));
    }
}