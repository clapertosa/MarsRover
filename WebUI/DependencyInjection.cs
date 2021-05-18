using Microsoft.Extensions.DependencyInjection;

namespace WebUI
{
    public static class DependencyInjection
    {
        public static void AddWebUI(this IServiceCollection services)
        {
            services.AddControllers();
        }
    }
}