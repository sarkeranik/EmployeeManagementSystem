namespace EmployeeManagement.SharedTestHelpers.Fakes.Item;

using EmployeeManagement.Domain.Items;
using EmployeeManagement.Domain.Items.Dtos;

public class FakeItemBuilder
{
    private ItemForCreationDto _creationData = new FakeItemForCreationDto().Generate();

    public FakeItemBuilder WithDto(ItemForCreationDto dto)
    {
        _creationData = dto;
        return this;
    }
    
    public Item Build()
    {
        var result = Item.Create(_creationData);
        return result;
    }
}