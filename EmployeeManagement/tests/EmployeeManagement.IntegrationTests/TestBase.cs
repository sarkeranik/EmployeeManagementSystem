namespace EmployeeManagement.IntegrationTests;

using NUnit.Framework;
using System.Threading.Tasks;
using AutoBogus;
using static TestFixture;

[Parallelizable]
public class TestBase
{
    [SetUp]
    public Task TestSetUp()
    {
        AutoFaker.Configure(builder =>
        {
            // configure global autobogus settings here
            builder.WithDateTimeKind(DateTimeKind.Utc)
                .WithRecursiveDepth(3)
                .WithTreeDepth(1)
                .WithRepeatCount(1);
        });
        
        return Task.CompletedTask;
    }
}