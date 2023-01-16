namespace EmployeeManagement.UnitTests.UnitTests.Domain.Users;

using EmployeeManagement.SharedTestHelpers.Fakes.User;
using EmployeeManagement.Domain.Users;
using EmployeeManagement.Domain.Users.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateUserTests
{
    private readonly Faker _faker;

    public CreateUserTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_user()
    {
        // Arrange
        var userToCreate = new FakeUserForCreationDto().Generate();
        
        // Act
        var fakeUser = User.Create(userToCreate);

        // Assert
        fakeUser.Name.Should().Be(userToCreate.Name);
        fakeUser.UserProfileId.Should().Be(userToCreate.UserProfileId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeUser = FakeUser.Generate();

        // Assert
        fakeUser.DomainEvents.Count.Should().Be(1);
        fakeUser.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserCreated));
    }
}