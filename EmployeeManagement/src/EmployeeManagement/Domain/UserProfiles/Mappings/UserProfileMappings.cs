namespace EmployeeManagement.Domain.UserProfiles.Mappings;

using EmployeeManagement.Domain.UserProfiles.Dtos;
using EmployeeManagement.Domain.UserProfiles;
using Mapster;

public sealed class UserProfileMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserProfile, UserProfileDto>();
        config.NewConfig<UserProfileForCreationDto, UserProfile>()
            .TwoWays();
        config.NewConfig<UserProfileForUpdateDto, UserProfile>()
            .TwoWays();
    }
}