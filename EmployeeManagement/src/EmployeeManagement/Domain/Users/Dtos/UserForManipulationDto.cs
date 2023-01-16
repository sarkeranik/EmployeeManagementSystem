namespace EmployeeManagement.Domain.Users.Dtos;

public abstract class UserForManipulationDto 
{
        public string Name { get; set; }
        public Guid? UserProfileId { get; set; }
}
