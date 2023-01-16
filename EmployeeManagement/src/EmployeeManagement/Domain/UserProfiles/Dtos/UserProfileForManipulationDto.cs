namespace EmployeeManagement.Domain.UserProfiles.Dtos;

public abstract class UserProfileForManipulationDto 
{
        public string Name { get; set; }
        public Guid UserId { get; set; }
}
