namespace EmployeeManagement.IntegrationTests.FeatureTests.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using EmployeeManagement.Domain.Items.Dtos;
using SharedKernel.Exceptions;
using EmployeeManagement.Domain.Items.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class UpdateItemCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_item_in_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeItemOne = FakeItem.Generate(new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id).Generate());
        var updatedItemDto = new FakeItemForUpdateDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id).Generate();
        await InsertAsync(fakeItemOne);

        var item = await ExecuteDbContextAsync(db => db.Items
            .FirstOrDefaultAsync(i => i.Id == fakeItemOne.Id));
        var id = item.Id;

        // Act
        var command = new UpdateItem.Command(id, updatedItemDto);
        await SendAsync(command);
        var updatedItem = await ExecuteDbContextAsync(db => db.Items.FirstOrDefaultAsync(i => i.Id == id));

        // Assert
        updatedItem.Name.Should().Be(updatedItemDto.Name);
        updatedItem.Description.Should().Be(updatedItemDto.Description);
        updatedItem.ProjectId.Should().Be(updatedItemDto.ProjectId);
    }
}