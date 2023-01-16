namespace EmployeeManagement.IntegrationTests.FeatureTests.Items;

using EmployeeManagement.Domain.Items.Dtos;
using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Items.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class ItemListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_item_list()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        var fakeProjectTwo = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne, fakeProjectTwo);

        var fakeItemOne = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id).Generate());
        var fakeItemTwo = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectTwo.Id).Generate());
        var queryParameters = new ItemParametersDto();

        await InsertAsync(fakeItemOne, fakeItemTwo);

        // Act
        var query = new GetItemList.Query(queryParameters);
        var items = await SendAsync(query);

        // Assert
        items.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}