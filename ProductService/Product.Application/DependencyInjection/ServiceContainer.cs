using Microsoft.Extensions.DependencyInjection;
using Product.Application.Services;
namespace Product.Application.DependencyInjection
{
    public static class ServiceContainer
    {

        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
