namespace EmployeeManagement.Domain.Clients;

using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.Domain.Clients.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using EmployeeManagement.Domain.Projects;


public class Client : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Address { get; private set; }

    [JsonIgnore, IgnoreDataMember]
    [ForeignKey("Project")]
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual Guid? ProjectId { get; private set; }
    public virtual Project Project { get; private set; }


    public static Client Create(ClientForCreationDto clientForCreationDto)
    {
        var newClient = new Client();

        newClient.Name = clientForCreationDto.Name;
        newClient.Address = clientForCreationDto.Address;
        newClient.ProjectId = clientForCreationDto.ProjectId;

        newClient.QueueDomainEvent(new ClientCreated(){ Client = newClient });
        
        return newClient;
    }

    public Client Update(ClientForUpdateDto clientForUpdateDto)
    {
        Name = clientForUpdateDto.Name;
        Address = clientForUpdateDto.Address;
        ProjectId = clientForUpdateDto.ProjectId;

        QueueDomainEvent(new ClientUpdated(){ Id = Id });
        return this;
    }
    
    protected Client() { } // For EF + Mocking
}