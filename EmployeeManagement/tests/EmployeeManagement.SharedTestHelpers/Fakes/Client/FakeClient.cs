namespace EmployeeManagement.SharedTestHelpers.Fakes.Client;

using AutoBogus;
using EmployeeManagement.Domain.Clients;
using EmployeeManagement.Domain.Clients.Dtos;

public sealed class FakeClient
{
    public static Client Generate(ClientForCreationDto clientForCreationDto)
    {
        return Client.Create(clientForCreationDto);
    }

    public static Client Generate()
    {
        return Generate(new FakeClientForCreationDto().Generate());
    }
}