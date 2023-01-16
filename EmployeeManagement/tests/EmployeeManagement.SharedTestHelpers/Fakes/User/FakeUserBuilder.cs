namespace EmployeeManagement.SharedTestHelpers.Fakes.User;

using EmployeeManagement.Domain.Users;
using EmployeeManagement.Domain.Users.Dtos;

public class FakeUserBuilder
{
    private UserForCreationDto _creationData = new FakeUserForCreationDto().Generate();

    public FakeUserBuilder WithDto(UserForCreationDto dto)
    {
        _creationData = dto;
        return this;
    }
    
    public User Build()
    {
        var result = User.Create(_creationData);
        return result;
    }
}