namespace EmployeeManagement.Domain.Clients.DomainEvents;

public sealed class ClientUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            