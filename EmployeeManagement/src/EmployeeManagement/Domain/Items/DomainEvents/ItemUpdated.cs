namespace EmployeeManagement.Domain.Items.DomainEvents;

public sealed class ItemUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            