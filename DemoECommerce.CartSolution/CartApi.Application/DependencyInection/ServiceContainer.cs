using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using CartApi.Application.Services;
using Polly.Retry;

namespace CartApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            // Register HttpClient with typed client
            services.AddHttpClient<ICartService, CartService>(client =>
            {
                client.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Configure resilience pipeline
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    OnRetry = args =>
                    {
                        Console.WriteLine($"Retry attempt {args.AttemptNumber}");
                        return ValueTask.CompletedTask;
                    }
                });
            });

            return services;
        }
    }
}