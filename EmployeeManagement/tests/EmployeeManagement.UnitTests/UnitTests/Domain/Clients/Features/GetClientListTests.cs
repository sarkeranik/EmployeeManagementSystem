namespace EmployeeManagement.UnitTests.UnitTests.Domain.Clients.Features;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.Domain.Clients;
using EmployeeManagement.Domain.Clients.Dtos;
using EmployeeManagement.Domain.Clients.Mappings;
using EmployeeManagement.Domain.Clients.Features;
using EmployeeManagement.Domain.Clients.Services;
using MapsterMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetClientListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IClientRepository> _clientRepository;

    public GetClientListTests()
    {
        _clientRepository = new Mock<IClientRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
    }
    
    [Test]
    public async Task can_get_paged_list_of_client()
    {
        //Arrange
        var fakeClientOne = FakeClient.Generate();
        var fakeClientTwo = FakeClient.Generate();
        var fakeClientThree = FakeClient.Generate();
        var client = new List<Client>();
        client.Add(fakeClientOne);
        client.Add(fakeClientTwo);
        client.Add(fakeClientThree);
        var mockDbData = client.AsQueryable().BuildMock();
        
        var queryParameters = new ClientParametersDto() { PageSize = 1, PageNumber = 2 };

        _clientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetClientList.Query(queryParameters);
        var handler = new GetClientList.Handler(_clientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_client_list_using_Name()
    {
        //Arrange
        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.Name, _ => "alpha")
            .Generate());
        var fakeClientTwo = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.Name, _ => "bravo")
            .Generate());
        var queryParameters = new ClientParametersDto() { Filters = $"Name == {fakeClientTwo.Name}" };

        var clientList = new List<Client>() { fakeClientOne, fakeClientTwo };
        var mockDbData = clientList.AsQueryable().BuildMock();

        _clientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetClientList.Query(queryParameters);
        var handler = new GetClientList.Handler(_clientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeClientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_client_list_using_Address()
    {
        //Arrange
        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.Address, _ => "alpha")
            .Generate());
        var fakeClientTwo = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.Address, _ => "bravo")
            .Generate());
        var queryParameters = new ClientParametersDto() { Filters = $"Address == {fakeClientTwo.Address}" };

        var clientList = new List<Client>() { fakeClientOne, fakeClientTwo };
        var mockDbData = clientList.AsQueryable().BuildMock();

        _clientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetClientList.Query(queryParameters);
        var handler = new GetClientList.Handler(_clientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeClientTwo, options =>
                options.ExcludingMissingMembers());
    }



    [Test]
    public async Task can_get_sorted_list_of_client_by_Name()
    {
        //Arrange
        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.Name, _ => "alpha")
            .Generate());
        var fakeClientTwo = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.Name, _ => "bravo")
            .Generate());
        var queryParameters = new ClientParametersDto() { SortOrder = "-Name" };

        var ClientList = new List<Client>() { fakeClientOne, fakeClientTwo };
        var mockDbData = ClientList.AsQueryable().BuildMock();

        _clientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetClientList.Query(queryParameters);
        var handler = new GetClientList.Handler(_clientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeClientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeClientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_client_by_Address()
    {
        //Arrange
        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.Address, _ => "alpha")
            .Generate());
        var fakeClientTwo = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.Address, _ => "bravo")
            .Generate());
        var queryParameters = new ClientParametersDto() { SortOrder = "-Address" };

        var ClientList = new List<Client>() { fakeClientOne, fakeClientTwo };
        var mockDbData = ClientList.AsQueryable().BuildMock();

        _clientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetClientList.Query(queryParameters);
        var handler = new GetClientList.Handler(_clientRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeClientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeClientOne, options =>
                options.ExcludingMissingMembers());
    }


}