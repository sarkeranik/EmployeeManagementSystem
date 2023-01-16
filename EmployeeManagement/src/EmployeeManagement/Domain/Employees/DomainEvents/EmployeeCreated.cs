namespace EmployeeManagement.Domain.Employees.DomainEvents;

public sealed class EmployeeCreated : DomainEvent
{
    public Employee Employee { get; set; } 
}
            