namespace EmployeeManagement.Domain.Items.Dtos;

public sealed class ItemDto 
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ProjectId { get; set; }
}
