namespace EmployeeManagement.UnitTests.UnitTests.Domain.UserProfiles;

using EmployeeManagement.SharedTestHelpers.Fakes.UserProfile;
using EmployeeManagement.Domain.UserProfiles;
using EmployeeManagement.Domain.UserProfiles.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateUserProfileTests
{
    private readonly Faker _faker;

    public UpdateUserProfileTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_userProfile()
    {
        // Arrange
        var fakeUserProfile = FakeUserProfile.Generate();
        var updatedUserProfile = new FakeUserProfileForUpdateDto().Generate();
        
        // Act
        fakeUserProfile.Update(updatedUserProfile);

        // Assert
        fakeUserProfile.Name.Should().Be(updatedUserProfile.Name);
        fakeUserProfile.UserId.Should().Be(updatedUserProfile.UserId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeUserProfile = FakeUserProfile.Generate();
        var updatedUserProfile = new FakeUserProfileForUpdateDto().Generate();
        fakeUserProfile.DomainEvents.Clear();
        
        // Act
        fakeUserProfile.Update(updatedUserProfile);

        // Assert
        fakeUserProfile.DomainEvents.Count.Should().Be(1);
        fakeUserProfile.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(UserProfileUpdated));
    }
}