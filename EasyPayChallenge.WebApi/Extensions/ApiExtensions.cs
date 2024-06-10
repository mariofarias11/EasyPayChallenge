using EasyPayChallenge.Application.Handlers;
using EasyPayChallenge.Domain.Models.Commands;
using MediatR;

namespace EasyPayChallenge.WebApi.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateProductHandler)));
            services.AddScoped<IMediator, Mediator>();

            return services;
        }
    }
}
