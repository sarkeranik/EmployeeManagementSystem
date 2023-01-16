namespace EmployeeManagement.SharedTestHelpers.Fakes.Client;

using EmployeeManagement.Domain.Clients;
using EmployeeManagement.Domain.Clients.Dtos;

public class FakeClientBuilder
{
    private ClientForCreationDto _creationData = new FakeClientForCreationDto().Generate();

    public FakeClientBuilder WithDto(ClientForCreationDto dto)
    {
        _creationData = dto;
        return this;
    }
    
    public Client Build()
    {
        var result = Client.Create(_creationData);
        return result;
    }
}