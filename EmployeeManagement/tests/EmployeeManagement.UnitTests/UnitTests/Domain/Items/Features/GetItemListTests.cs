namespace EmployeeManagement.UnitTests.UnitTests.Domain.Items.Features;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.Domain.Items;
using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.Domain.Items.Mappings;
using EmployeeManagement.Domain.Items.Features;
using EmployeeManagement.Domain.Items.Services;
using MapsterMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetItemListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IItemRepository> _itemRepository;

    public GetItemListTests()
    {
        _itemRepository = new Mock<IItemRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
    }
    
    [Test]
    public async Task can_get_paged_list_of_item()
    {
        //Arrange
        var fakeItemOne = FakeItem.Generate();
        var fakeItemTwo = FakeItem.Generate();
        var fakeItemThree = FakeItem.Generate();
        var item = new List<Item>();
        item.Add(fakeItemOne);
        item.Add(fakeItemTwo);
        item.Add(fakeItemThree);
        var mockDbData = item.AsQueryable().BuildMock();
        
        var queryParameters = new ItemParametersDto() { PageSize = 1, PageNumber = 2 };

        _itemRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetItemList.Query(queryParameters);
        var handler = new GetItemList.Handler(_itemRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_item_list_using_Name()
    {
        //Arrange
        var fakeItemOne = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.Name, _ => "alpha")
            .Generate());
        var fakeItemTwo = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.Name, _ => "bravo")
            .Generate());
        var queryParameters = new ItemParametersDto() { Filters = $"Name == {fakeItemTwo.Name}" };

        var itemList = new List<Item>() { fakeItemOne, fakeItemTwo };
        var mockDbData = itemList.AsQueryable().BuildMock();

        _itemRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetItemList.Query(queryParameters);
        var handler = new GetItemList.Handler(_itemRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeItemTwo, options =>
                options.ExcludingMissingMembers());
    }



    [Test]
    public async Task can_get_sorted_list_of_item_by_Name()
    {
        //Arrange
        var fakeItemOne = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.Name, _ => "alpha")
            .Generate());
        var fakeItemTwo = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.Name, _ => "bravo")
            .Generate());
        var queryParameters = new ItemParametersDto() { SortOrder = "-Name" };

        var ItemList = new List<Item>() { fakeItemOne, fakeItemTwo };
        var mockDbData = ItemList.AsQueryable().BuildMock();

        _itemRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetItemList.Query(queryParameters);
        var handler = new GetItemList.Handler(_itemRepository.Object, _mapper, _sieveProcessor);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeItemTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeItemOne, options =>
                options.ExcludingMissingMembers());
    }


}