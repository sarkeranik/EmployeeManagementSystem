namespace EmployeeManagement.Extensions.Services;

using EmployeeManagement.Databases;
using EmployeeManagement.Resources;
using EmployeeManagement.Services;
using Microsoft.EntityFrameworkCore;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IWebHostEnvironment env)
    {
        // DbContext -- Do Not Delete
        var connectionString = EnvironmentService.DbConnectionString;
        if(string.IsNullOrWhiteSpace(connectionString))
        {
            // this makes local migrations easier to manage. feel free to refactor if desired.
            connectionString = env.IsDevelopment() 
                ? "Server=ANIKSARKER\\SQLEXPRESS;Database=EmployeeManagement;Trusted_Connection=True;"
                : throw new Exception("DB_CONNECTION_STRING environment variable is not set.");
        }

        services.AddDbContext<EmployeesDbContext>(options =>
            options.UseSqlServer(connectionString,
                builder => builder.MigrationsAssembly(typeof(EmployeesDbContext).Assembly.FullName))
                            .UseSnakeCaseNamingConvention());

        services.AddHostedService<MigrationHostedService<EmployeesDbContext>>();

        // Auth -- Do Not Delete
    }
}
