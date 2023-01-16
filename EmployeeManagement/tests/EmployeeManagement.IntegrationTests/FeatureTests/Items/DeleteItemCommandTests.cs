namespace EmployeeManagement.IntegrationTests.FeatureTests.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.Domain.Items.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class DeleteItemCommandTests : TestBase
{
    [Test]
    public async Task can_delete_item_from_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeItemOne = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id).Generate());
        await InsertAsync(fakeItemOne);
        var item = await ExecuteDbContextAsync(db => db.Items
            .FirstOrDefaultAsync(i => i.Id == fakeItemOne.Id));

        // Act
        var command = new DeleteItem.Command(item.Id);
        await SendAsync(command);
        var itemResponse = await ExecuteDbContextAsync(db => db.Items.CountAsync(i => i.Id == item.Id));

        // Assert
        itemResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_item_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteItem.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_item_from_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeItemOne = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id).Generate());
        await InsertAsync(fakeItemOne);
        var item = await ExecuteDbContextAsync(db => db.Items
            .FirstOrDefaultAsync(i => i.Id == fakeItemOne.Id));

        // Act
        var command = new DeleteItem.Command(item.Id);
        await SendAsync(command);
        var deletedItem = await ExecuteDbContextAsync(db => db.Items
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == item.Id));

        // Assert
        deletedItem?.IsDeleted.Should().BeTrue();
    }
}