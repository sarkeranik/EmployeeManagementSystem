namespace EmployeeManagement.Domain.UserProfiles.DomainEvents;

public sealed class UserProfileCreated : DomainEvent
{
    public UserProfile UserProfile { get; set; } 
}
            