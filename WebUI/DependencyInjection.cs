using Microsoft.Extensions.DependencyInjection;

namespace WebUI
{
    public static class DependencyInjection
    {
        public static void AddWebUI(this IServiceCollection services, string corsPolicy)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy,
                    builder => { builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowCredentials(); });
            });
        }
    }
}