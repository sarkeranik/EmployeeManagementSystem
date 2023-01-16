namespace EmployeeManagement.Domain.Projects.DomainEvents;

public sealed class ProjectCreated : DomainEvent
{
    public Project Project { get; set; } 
}
            