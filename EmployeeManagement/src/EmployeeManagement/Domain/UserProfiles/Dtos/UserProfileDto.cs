namespace EmployeeManagement.Domain.UserProfiles.Dtos;

public sealed class UserProfileDto 
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
}
