using Microsoft.Extensions.DependencyInjection;
using RecAI.Application.Interfaces;
using RecAI.Application.Services;

namespace RecAI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRecommendationService, RecommendationService>();
        services.AddScoped<IDashboardService, DashboardService>();
        return services;
    }
}