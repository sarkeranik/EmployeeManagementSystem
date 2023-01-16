namespace EmployeeManagement.SharedTestHelpers.Fakes.User;

using AutoBogus;
using EmployeeManagement.Domain.Users;
using EmployeeManagement.Domain.Users.Dtos;

public sealed class FakeUser
{
    public static User Generate(UserForCreationDto userForCreationDto)
    {
        return User.Create(userForCreationDto);
    }

    public static User Generate()
    {
        return Generate(new FakeUserForCreationDto().Generate());
    }
}