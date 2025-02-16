using Authentication.Application.Interfaces;
using Authentication.Infrastructure.Data;
using Authentication.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {

        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Database Connection
            // Add Authentication scheme
            SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(services, configuration);

            // Add Dependency Injection
            services.AddScoped<IUser, UserRepository>();

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
