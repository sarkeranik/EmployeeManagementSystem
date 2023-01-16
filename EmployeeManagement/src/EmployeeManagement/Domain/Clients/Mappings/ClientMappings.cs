namespace EmployeeManagement.Domain.Clients.Mappings;

using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.Domain.Clients;
using Mapster;

public sealed class ClientMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Client, ClientDto>();
        config.NewConfig<ClientForCreationDto, Client>()
            .TwoWays();
        config.NewConfig<ClientForUpdateDto, Client>()
            .TwoWays();
    }
}