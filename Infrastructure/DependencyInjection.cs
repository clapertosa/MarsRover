using Application.Interfaces.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IRoverRepository, RoverRepository>();
            services.AddScoped<IObstacleRepository, ObstacleRepository>();
        }
    }
}