namespace EmployeeManagement.Domain.Items;

using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.Domain.Items.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using EmployeeManagement.Domain.Projects;


public class Item : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }

    public virtual string Description { get; private set; }

    [JsonIgnore, IgnoreDataMember]
    [ForeignKey("Project")]
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual Guid? ProjectId { get; private set; }
    public virtual Project Project { get; private set; }


    public static Item Create(ItemForCreationDto itemForCreationDto)
    {
        var newItem = new Item();

        newItem.Name = itemForCreationDto.Name;
        newItem.Description = itemForCreationDto.Description;
        newItem.ProjectId = itemForCreationDto.ProjectId;

        newItem.QueueDomainEvent(new ItemCreated(){ Item = newItem });
        
        return newItem;
    }

    public Item Update(ItemForUpdateDto itemForUpdateDto)
    {
        Name = itemForUpdateDto.Name;
        Description = itemForUpdateDto.Description;
        ProjectId = itemForUpdateDto.ProjectId;

        QueueDomainEvent(new ItemUpdated(){ Id = Id });
        return this;
    }
    
    protected Item() { } // For EF + Mocking
}