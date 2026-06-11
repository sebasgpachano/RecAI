using Microsoft.Extensions.DependencyInjection;

namespace RecAI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services (e.g. IRecommendationService) get registered here in Phase 4.
        return services;
    }
}