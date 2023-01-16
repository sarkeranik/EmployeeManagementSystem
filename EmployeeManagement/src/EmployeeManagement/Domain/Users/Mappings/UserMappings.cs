namespace EmployeeManagement.Domain.Users.Mappings;

using EmployeeManagement.Domain.Users.Dtos;
using EmployeeManagement.Domain.Users;
using Mapster;

public sealed class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>();
        config.NewConfig<UserForCreationDto, User>()
            .TwoWays();
        config.NewConfig<UserForUpdateDto, User>()
            .TwoWays();
    }
}