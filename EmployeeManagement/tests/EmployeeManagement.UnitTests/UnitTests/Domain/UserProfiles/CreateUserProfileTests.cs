namespace EmployeeManagement.UnitTests.UnitTests.Domain.UserProfiles;

using EmployeeManagement.SharedTestHelpers.Fakes.UserProfile;
using EmployeeManagement.Domain.UserProfiles;
using EmployeeManagement.Domain.UserProfiles.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateUserProfileTests
{
    private readonly Faker _faker;

    public CreateUserProfileTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_userProfile()
    {
        // Arrange
        var userProfileToCreate = new FakeUserProfileForCreationDto().Generate();
        
        // Act
        var fakeUserProfile = UserProfile.Create(userProfileToCreate);

        // Assert
        fakeUserProfile.Name.Should().Be(userProfileToCreate.Name);
        fakeUserProfile.UserId.Should().Be(userProfileToCreate.UserId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeUserProfile = FakeUserProfile.Generate();

        // Assert
        fakeUserProfile.DomainEvents.Count.Should().Be(1);
        fakeUserProfile.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserProfileCreated));
    }
}