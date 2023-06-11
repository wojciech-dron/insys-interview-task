using System.Reflection;
using AutoMapper;
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
        services.AddSingleton(GetMapper(assembly));

        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<IMovieQueries, MovieQueries>();
    }

    internal static IMapper GetMapper(Assembly assembly)
    {
        var mapperConfiguration = new MapperConfiguration(config 
            => { config.AddMaps(assembly); });
        
        mapperConfiguration.AssertConfigurationIsValid();
        
        return mapperConfiguration.CreateMapper();
    }
}