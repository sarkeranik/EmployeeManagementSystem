namespace EmployeeManagement.IntegrationTests.FeatureTests.Clients;

using EmployeeManagement.SharedTestHelpers.Fakes.Client;
using EmployeeManagement.Domain.Clients.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using EmployeeManagement.SharedTestHelpers.Fakes.Project;

public class ClientQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_client_with_accurate_props()
    {
        // Arrange
        var fakeProjectOne = FakeProject.Generate(new FakeProjectForCreationDto().Generate());
        await InsertAsync(fakeProjectOne);

        var fakeClientOne = FakeClient.Generate(new FakeClientForCreationDto()
            .RuleFor(c => c.ProjectId, _ => fakeProjectOne.Id).Generate());
        await InsertAsync(fakeClientOne);

        // Act
        var query = new GetClient.Query(fakeClientOne.Id);
        var client = await SendAsync(query);

        // Assert
        client.Name.Should().Be(fakeClientOne.Name);
        client.Address.Should().Be(fakeClientOne.Address);
        client.ProjectId.Should().Be(fakeClientOne.ProjectId);
    }

    [Test]
    public async Task get_client_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetClient.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}