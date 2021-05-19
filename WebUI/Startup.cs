using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebUI.Middlewares;

namespace WebUI
{
    public class Startup
    {
        private readonly string CorsPolicy = "Cors Policy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();
            services.AddWebUI(CorsPolicy);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}