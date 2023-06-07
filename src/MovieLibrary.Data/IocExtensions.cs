using Microsoft.Extensions.DependencyInjection;
using MovieLibrary.Data.Repositories;

namespace MovieLibrary.Data;

public static class IocExtensions
{
    public static void AddDataModule(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

}