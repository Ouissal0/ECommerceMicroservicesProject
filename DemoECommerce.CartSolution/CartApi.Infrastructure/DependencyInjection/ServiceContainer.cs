using CartApi.Application.Interfaces;
using CartApi.Application.Services;
using CartApi.Infrastructure.Data;
using CartApi.Infrastructure.Repositories;
using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace CartApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // Ajouter la connectivité à la base de données
            SharedServiceContainer.AddSharedServices<CartDbContext>(services, config, config["MySerilog:FileName"]!);

            // Ajouter uniquement le repository, pas le service
            services.AddScoped<ICart, CartRepository>();
           

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}