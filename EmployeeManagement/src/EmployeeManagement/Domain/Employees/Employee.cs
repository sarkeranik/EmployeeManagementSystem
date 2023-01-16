namespace EmployeeManagement.Domain.Employees;

using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Employees.Dtos;
using EmployeeManagement.Domain.Employees.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using EmployeeManagement.Domain.Users;


public class Employee : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual int Salary { get; private set; }

    public virtual string Designation { get; private set; }

    [Required]
    [JsonIgnore, IgnoreDataMember]
    [ForeignKey("User")]
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual Guid? EmployeeId { get; private set; }
    public virtual User User { get; private set; }


    public static Employee Create(EmployeeForCreationDto employeeForCreationDto)
    {
        var newEmployee = new Employee();

        newEmployee.Name = employeeForCreationDto.Name;
        newEmployee.Salary = employeeForCreationDto.Salary;
        newEmployee.Designation = employeeForCreationDto.Designation;
        newEmployee.EmployeeId = employeeForCreationDto.EmployeeId;

        newEmployee.QueueDomainEvent(new EmployeeCreated(){ Employee = newEmployee });
        
        return newEmployee;
    }

    public Employee Update(EmployeeForUpdateDto employeeForUpdateDto)
    {
        Name = employeeForUpdateDto.Name;
        Salary = employeeForUpdateDto.Salary;
        Designation = employeeForUpdateDto.Designation;
        EmployeeId = employeeForUpdateDto.EmployeeId;

        QueueDomainEvent(new EmployeeUpdated(){ Id = Id });
        return this;
    }
    
    protected Employee() { } // For EF + Mocking
}