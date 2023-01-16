namespace EmployeeManagement.Extensions.Application;

using EmployeeManagement.Middleware;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerUI;

public static class SwaggerAppExtension
{
    public static void UseSwaggerExtension(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.SwaggerEndpoint("/swagger/v1/swagger.json", "");
            config.DocExpansion(DocExpansion.None);
        });
    }
}