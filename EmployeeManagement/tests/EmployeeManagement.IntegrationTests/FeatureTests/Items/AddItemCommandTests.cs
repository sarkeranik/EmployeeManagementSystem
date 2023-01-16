namespace EmployeeManagement.IntegrationTests.FeatureTests.Items;

using EmployeeManagement.SharedTestHelpers.Fakes.Item;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Items.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class AddItemCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_item_to_db()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeItemOne = new FakeItemForCreationDto()
            .RuleFor(i => i.ProjectId, _ => fakeProjectOne.Id).Generate();

        // Act
        var command = new AddItem.Command(fakeItemOne);
        var itemReturned = await SendAsync(command);
        var itemCreated = await ExecuteDbContextAsync(db => db.Items
            .FirstOrDefaultAsync(i => i.Id == itemReturned.Id));

        // Assert
        itemReturned.Name.Should().Be(fakeItemOne.Name);
        itemReturned.Description.Should().Be(fakeItemOne.Description);
        itemReturned.ProjectId.Should().Be(fakeItemOne.ProjectId);

        itemCreated.Name.Should().Be(fakeItemOne.Name);
        itemCreated.Description.Should().Be(fakeItemOne.Description);
        itemCreated.ProjectId.Should().Be(fakeItemOne.ProjectId);
    }
}