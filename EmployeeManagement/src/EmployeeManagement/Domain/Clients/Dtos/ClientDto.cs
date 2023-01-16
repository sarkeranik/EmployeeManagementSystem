namespace EmployeeManagement.Domain.Clients.Dtos;

public sealed class ClientDto 
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Guid? ProjectId { get; set; }
}
