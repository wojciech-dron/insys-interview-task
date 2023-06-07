using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MovieLibrary.Core.Queries;

namespace MovieLibrary.Core;

public static class IocExtensions
{
    public static void AddCoreModule(this IServiceCollection services)
    {
        var assembly = typeof(IocExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        
        services.AddValidatorsFromAssembly(assembly);
        // Register validators for requests
        
        
        services.AddScoped<CategoryQueries>();
    }
}