using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Services;
using Polly;
using Polly.Retry;
using SharedLibrary.Logs;

namespace Order.Application.DependencyInjection
{
    public static class ServiceContainer
    {

        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            // Add HttpClient service
            // Create Dependency Injection for OrderService
            services.AddHttpClient<IOrderService, OrderService>(o =>
            {
                o.BaseAddress = new Uri(configuration["ApiGateway:BaseAddress"]!);
                o.Timeout = TimeSpan.FromSeconds(10);
            });

            // Create retry strategy
            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"OnRetry, Attempt: {args.AttemptNumber} Outcome {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                }
            };

            // Use Retry strategy
            services.AddResiliencePipeline("my-retry-pipeline", builder => 
            {
                builder.AddRetry(retryStrategy);
            });

            return services;
        }
    }
}
