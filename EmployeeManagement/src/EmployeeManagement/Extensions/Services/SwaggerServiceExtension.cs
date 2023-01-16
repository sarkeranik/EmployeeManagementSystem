namespace EmployeeManagement.Extensions.Services;

using EmployeeManagement.Services;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public static class SwaggerServiceExtension
{
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            config.CustomSchemaIds(type => type.ToString());
            config.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });

            config.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Employee Management System",
                    Description = "Our API uses a REST based design, leverages the JSON data format, and relies upon HTTPS for transport. We respond with meaningful HTTP response codes and if an error occurs, we include error details in the response body. API Documentation is at carbonkitchen.com/dev/docs",
                    Contact = new OpenApiContact
                    {
                        Name = "Employee Management System",
                        Email = "devsupport@ems.com",
                            Url = new Uri("https://www.ems.com"),
                    },
                });

            config.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}EmployeeManagement.WebApi.xml"));
        });
    }
}