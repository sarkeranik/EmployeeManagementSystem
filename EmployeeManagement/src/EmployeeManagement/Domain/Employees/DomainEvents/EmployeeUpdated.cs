namespace EmployeeManagement.Domain.Employees.DomainEvents;

public sealed class EmployeeUpdated : DomainEvent
{
    public Guid Id { get; set; } 
}
            