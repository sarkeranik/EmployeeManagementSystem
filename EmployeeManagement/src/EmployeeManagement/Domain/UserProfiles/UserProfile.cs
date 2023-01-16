namespace EmployeeManagement.Domain.UserProfiles;

using SharedKernel.Exceptions;
using EmployeeManagement.Domain.UserProfiles.Dtos;
using EmployeeManagement.Domain.UserProfiles.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using EmployeeManagement.Domain.Users;


public class UserProfile : BaseEntity
{
    public virtual string Name { get; private set; }

    [JsonIgnore, IgnoreDataMember]
    [ForeignKey("User")]
    public virtual Guid UserId { get; private set; }
    public virtual User User { get; private set; }


    public static UserProfile Create(UserProfileForCreationDto userProfileForCreationDto)
    {
        var newUserProfile = new UserProfile();

        newUserProfile.Name = userProfileForCreationDto.Name;
        newUserProfile.UserId = userProfileForCreationDto.UserId;

        newUserProfile.QueueDomainEvent(new UserProfileCreated(){ UserProfile = newUserProfile });
        
        return newUserProfile;
    }

    public UserProfile Update(UserProfileForUpdateDto userProfileForUpdateDto)
    {
        Name = userProfileForUpdateDto.Name;
        UserId = userProfileForUpdateDto.UserId;

        QueueDomainEvent(new UserProfileUpdated(){ Id = Id });
        return this;
    }
    
    protected UserProfile() { } // For EF + Mocking
}