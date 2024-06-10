using EasyPayChallenge.Application.Repositories;
using EasyPayChallenge.Infrastructure.DB.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyPayChallenge.Infrastructure.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductRepository, ProductRepository>(provider => 
            new ProductRepository(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}