namespace EmployeeManagement.Domain.Items.DomainEvents;

public sealed class ItemCreated : DomainEvent
{
    public Item Item { get; set; } 
}
            