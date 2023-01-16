namespace EmployeeManagement.FunctionalTests;

using Databases;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using DotNet.Testcontainers.Containers.Modules.Databases;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

[SetUpFixture]
public class FunctionalTestFixture
{
    public static IServiceScopeFactory ScopeFactory { get; private set; }
    public static WebApplicationFactory<Program> Factory  { get; private set; }
    private readonly TestcontainerDatabase _dbContainer = dbSetup();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _dbContainer.StartAsync();
        Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", _dbContainer.ConnectionString);
        
        Factory = new TestingWebApplicationFactory();
        ScopeFactory = Factory.Services.GetRequiredService<IServiceScopeFactory>();

        using var scope = ScopeFactory.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<EmployeesDbContext>();
        await db.Database.MigrateAsync();
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()  
    {
        await _dbContainer.DisposeAsync();
        await Factory.DisposeAsync();
    }

    private static TestcontainerDatabase dbSetup()
    {
        var isMacOs = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        var cpuArch = RuntimeInformation.ProcessArchitecture;
        var isRunningOnMacOsArm64 = isMacOs && cpuArch == Architecture.Arm64;
        
        var baseDb = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration()
            {
                Password = "#testingDockerPassword#",
            })
            .WithName($"FunctionalTesting_EmployeeManagement_0ab7738a-2337-4e0a-b573-cd5601182b24");
            
        if(isRunningOnMacOsArm64)
            baseDb.WithImage("mcr.microsoft.com/azure-sql-edge:latest");

        return baseDb.Build();
    }
}