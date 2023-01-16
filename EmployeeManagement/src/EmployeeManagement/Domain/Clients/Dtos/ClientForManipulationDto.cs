namespace EmployeeManagement.Domain.Clients.Dtos;

public abstract class ClientForManipulationDto 
{
        public string Name { get; set; }
        public string Address { get; set; }
        public Guid? ProjectId { get; set; }
}
