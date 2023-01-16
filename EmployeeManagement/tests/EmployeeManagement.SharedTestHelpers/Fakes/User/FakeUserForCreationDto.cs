namespace EmployeeManagement.SharedTestHelpers.Fakes.User;

using AutoBogus;
using EmployeeManagement.Domain.Users;
using EmployeeManagement.Domain.Users.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public sealed class FakeUserForCreationDto : AutoFaker<UserForCreationDto>
{
    public FakeUserForCreationDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(u => u.ExampleIntProperty, u => u.Random.Number(50, 100000));
        //RuleFor(u => u.ExampleDateProperty, u => u.Date.Past());
    }
}