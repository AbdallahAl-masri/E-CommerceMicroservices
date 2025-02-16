using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Interfaces;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories;
using SharedLibrary.DependencyInjection;

namespace Order.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Database Connection
            // Add Authentication scheme
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, configuration);

            // Add Dependency Injection
            services.AddScoped<IOrder, OrderRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructureService(this IApplicationBuilder app)
        {
            // Register middleware
            // Global Exception Handler
            // Block outsider requests
            SharedServiceContainer.UseSharedServices(app);

            return app;
        }
    }
}
