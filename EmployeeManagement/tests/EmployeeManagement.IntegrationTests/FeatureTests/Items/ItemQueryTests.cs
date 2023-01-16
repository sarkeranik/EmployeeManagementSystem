namespace EmployeeManagement.IntegrationTests.FeatureTests.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.Domain.Items.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class ItemQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_item_with_accurate_props()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeItemOne = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id).Generate());
        await InsertAsync(fakeItemOne);

        // Act
        var query = new GetItem.Query(fakeItemOne.Id);
        var item = await SendAsync(query);

        // Assert
        item.Name.Should().Be(fakeItemOne.Name);
        item.Description.Should().Be(fakeItemOne.Description);
        item.ProjectId.Should().Be(fakeItemOne.ProjectId);
    }

    [Test]
    public async Task get_item_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetItem.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}