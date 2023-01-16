namespace EmployeeManagement.Domain.Items.Dtos;

public abstract class ItemForManipulationDto 
{
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ProjectId { get; set; }
}
