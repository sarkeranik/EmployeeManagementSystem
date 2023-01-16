namespace EmployeeManagement.Domain.Clients.DomainEvents;

public sealed class ClientCreated : DomainEvent
{
    public Client Client { get; set; } 
}
            