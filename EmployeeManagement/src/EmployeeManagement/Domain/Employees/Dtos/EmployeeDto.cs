namespace EmployeeManagement.Domain.Employees.Dtos;

public sealed class EmployeeDto 
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Designation { get; set; }
        public Guid? EmployeeId { get; set; }
}
