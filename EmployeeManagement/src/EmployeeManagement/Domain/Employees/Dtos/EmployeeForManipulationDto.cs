namespace EmployeeManagement.Domain.Employees.Dtos;

public abstract class EmployeeForManipulationDto 
{
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Designation { get; set; }
        public Guid? EmployeeId { get; set; }
}
