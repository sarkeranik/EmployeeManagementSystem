namespace EmployeeManagement.Domain.Projects;

using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Projects.Dtos;
using EmployeeManagement.Domain.Projects.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;


public class Project : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }

    public virtual string Description { get; private set; }


    public static Project Create(ProjectForCreationDto projectForCreationDto)
    {
        var newProject = new Project();

        newProject.Name = projectForCreationDto.Name;
        newProject.Description = projectForCreationDto.Description;

        newProject.QueueDomainEvent(new ProjectCreated(){ Project = newProject });
        
        return newProject;
    }

    public Project Update(ProjectForUpdateDto projectForUpdateDto)
    {
        Name = projectForUpdateDto.Name;
        Description = projectForUpdateDto.Description;

        QueueDomainEvent(new ProjectUpdated(){ Id = Id });
        return this;
    }
    
    protected Project() { } // For EF + Mocking
}