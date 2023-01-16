namespace EmployeeManagement.SharedTestHelpers.Fakes.Item;

using AutoBogus;
using EmployeeManagement.Domain.Items;
using EmployeeManagement.Domain.Items.Dtos;

public sealed class FakeItem
{
    public static Item Generate(ItemForCreationDto itemForCreationDto)
    {
        return Item.Create(itemForCreationDto);
    }

    public static Item Generate()
    {
        return Generate(new FakeItemForCreationDto().Generate());
    }
}