namespace EmployeeManagement.Domain.Items.Mappings;

using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.Domain.Items;
using Mapster;

public sealed class ItemMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Item, ItemDto>();
        config.NewConfig<ItemForCreationDto, Item>()
            .TwoWays();
        config.NewConfig<ItemForUpdateDto, Item>()
            .TwoWays();
    }
}