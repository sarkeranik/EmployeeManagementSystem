namespace EmployeeManagement.SharedTestHelpers.Fakes.UserProfile;

using AutoBogus;
using EmployeeManagement.Domain.UserProfiles;
using EmployeeManagement.Domain.UserProfiles.Dtos;

public sealed class FakeUserProfile
{
    public static UserProfile Generate(UserProfileForCreationDto userProfileForCreationDto)
    {
        return UserProfile.Create(userProfileForCreationDto);
    }

    public static UserProfile Generate()
    {
        return Generate(new FakeUserProfileForCreationDto().Generate());
    }
}