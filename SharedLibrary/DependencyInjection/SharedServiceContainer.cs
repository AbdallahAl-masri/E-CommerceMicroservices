using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedLibrary.Middleware;

namespace SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            // Add Generic DbContext
            services.AddDbContext<TContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("eCommerce"),
                sqlServerOption => sqlServerOption.EnableRetryOnFailure()));

            // config serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();

            // Add JWT Authentication Scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, configuration);
            return services;
        }

        public static IApplicationBuilder UseSharedServices(this IApplicationBuilder app)
        {
            // Use Global Exception Handler
            app.UseMiddleware<GlobalException>();

            // Register middleware to block all outsider requests
            app.UseMiddleware<OnlyListenToAPIGateway>();

            return app;
        }
    }
}
