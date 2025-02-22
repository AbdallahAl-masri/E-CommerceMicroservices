using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Interfaces;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repositories;
using SharedLibrary.DependencyInjection;

namespace Product.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {

        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Database Connection
            // Add Authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, configuration);

            // Add Repositories
            services.AddScoped<IProduct, ProductRepository>();

            // Add Swagger Documentation
            services.AddSwaggerDocumentation();
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
