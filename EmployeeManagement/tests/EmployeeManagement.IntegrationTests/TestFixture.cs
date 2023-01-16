namespace EmployeeManagement.IntegrationTests;

using EmployeeManagement.Extensions.Services;
using EmployeeManagement.Databases;
using EmployeeManagement;
using EmployeeManagement.Services;
using EmployeeManagement.Resources;
using EmployeeManagement.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using DotNet.Testcontainers.Containers.Modules.Databases;
using FluentAssertions;
using FluentAssertions.Extensions;

[SetUpFixture]
public class TestFixture
{
    private static IServiceScopeFactory _scopeFactory;
    private static ServiceProvider _provider;
    private readonly TestcontainerDatabase _dbContainer = dbSetup();

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        await _dbContainer.StartAsync();
        Environment.SetEnvironmentVariable("DB_CONNECTION_STRING", $"{_dbContainer.ConnectionString}TrustServerCertificate=true;");
        
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
        {
            EnvironmentName = Consts.Testing.IntegrationTestingEnvName,
        });
        builder.Configuration.AddEnvironmentVariables();

        builder.ConfigureServices();
        var services = builder.Services;

        // add any mock services here
        services.ReplaceServiceWithSingletonMock<IHttpContextAccessor>();

        // MassTransit Harness Setup -- Do Not Delete Comment

        _provider = services.BuildServiceProvider();
        _scopeFactory = _provider.GetService<IServiceScopeFactory>();

        // MassTransit Start Setup -- Do Not Delete Comment


        await EnsureDatabase();
        await ResetState();
    }

    private static async Task EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<EmployeesDbContext>();

        await context?.Database?.MigrateAsync();
    }

    public static TScopedService GetService<TScopedService>()
    {
        var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetService<TScopedService>();
        return service;
    }


    public static void SetUserRole(string role, string sub = null)
    {
        sub ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, sub)
        };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }

    public static void SetUserRoles(string[] roles, string sub = null)
    {
        sub ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>();
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        claims.Add(new Claim(ClaimTypes.Name, sub));

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }
    
    public static void SetMachineRole(string role, string clientId = null)
    {
        clientId ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim("client_role", role),
            new Claim("client_id", clientId)
        };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }

    public static void SetMachineRoles(string[] roles, string clientId = null)
    {
        clientId ??= Guid.NewGuid().ToString();
        var claims = new List<Claim>();
        foreach (var role in roles)
        {
            claims.Add(new Claim("client_role", role));
        }
        claims.Add(new Claim("client_id", clientId));

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = Mock.Of<HttpContext>(c => c.User == claimsPrincipal);

        var httpContextAccessor = GetService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task ResetState()
    {
        try
        {
            var respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
            });
            await respawner.ResetAsync(EnvironmentService.DbConnectionString);
        }
        catch (InvalidOperationException e)
        {
            throw new Exception($"There was an issue resetting your database state. You might need to add a migration to your project. You can add a migration with `dotnet ef migration add YourMigrationDescription`. More details on this error: {e.Message}");
        }
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<EmployeesDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<EmployeesDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmployeesDbContext>();

        try
        {
            //await dbContext.BeginTransactionAsync();

            await action(scope.ServiceProvider);

            //await dbContext.CommitTransactionAsync();
        }
        catch (Exception)
        {
            //dbContext.RollbackTransaction();
            throw;
        }
    }

    public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmployeesDbContext>();

        try
        {
            //await dbContext.BeginTransactionAsync();

            var result = await action(scope.ServiceProvider);

            //await dbContext.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            //dbContext.RollbackTransaction();
            throw;
        }
    }

    public static Task ExecuteDbContextAsync(Func<EmployeesDbContext, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<EmployeesDbContext>()));

    public static Task ExecuteDbContextAsync(Func<EmployeesDbContext, ValueTask> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<EmployeesDbContext>()).AsTask());

    public static Task ExecuteDbContextAsync(Func<EmployeesDbContext, IMediator, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<EmployeesDbContext>(), sp.GetService<IMediator>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<EmployeesDbContext, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<EmployeesDbContext>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<EmployeesDbContext, ValueTask<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<EmployeesDbContext>()).AsTask());

    public static Task<T> ExecuteDbContextAsync<T>(Func<EmployeesDbContext, IMediator, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<EmployeesDbContext>(), sp.GetService<IMediator>()));

    public static Task<int> InsertAsync<T>(params T[] entities) where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            return db.SaveChangesAsync();
        });
    }

    // MassTransit Methods -- Do Not Delete Comment

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
            .WithName($"IntegrationTesting_EmployeeManagement_be86fb15-c9d2-4c87-8805-379965685de7");
            
        if(isRunningOnMacOsArm64)
            baseDb.WithImage("mcr.microsoft.com/azure-sql-edge:latest");

        return baseDb.Build();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _dbContainer.DisposeAsync();
        
        // MassTransit Teardown -- Do Not Delete Comment
    }
}



public static class ServiceCollectionServiceExtensions
{
    public static IServiceCollection ReplaceServiceWithSingletonMock<TService>(this IServiceCollection services)
        where TService : class
    {
        services.RemoveAll(typeof(TService));
        services.AddSingleton(_ => Mock.Of<TService>());
        return services;
    }
}
