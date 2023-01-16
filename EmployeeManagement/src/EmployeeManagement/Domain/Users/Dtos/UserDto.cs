namespace EmployeeManagement.Domain.Users.Dtos;

public sealed class UserDto 
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? UserProfileId { get; set; }
}
