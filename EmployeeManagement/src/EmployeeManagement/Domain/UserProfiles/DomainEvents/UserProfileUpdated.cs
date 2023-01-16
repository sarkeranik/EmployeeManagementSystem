namespace EmployeeManagement.Domain.UserProfiles.DomainEvents;

public sealed class UserProfileUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            