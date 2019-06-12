using DriverService.Config;
using DriverService.Handlers;
using DriverService.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ShareMe.Carefully.Rabbit;

namespace DriverService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Setup singleton configs
            CreateSingletonConfigSettings<MongoSettings>(services, "mongoSettings");
            CreateSingletonConfigSettings<RabbitSettings>(services, "rabbitSettings");            

            // It's ok to have MongoClient as a singleton
            services.AddSingleton<EventStoreService>();
            services.AddSingleton<ReadModelService>();

            // Add RabbitHandler as Singleton (shutdown handled in hosted service)
            services.AddSingleton(s => new RabbitHandler(s.GetRequiredService<RabbitSettings>().Uri));
            services.AddTransient<RabbitMessenger>();
            services.AddScoped<MessengerService>();

            // Add Handlers as Singletons
            services.AddSingleton<DriverHiredHandler>();
            services.AddSingleton<LoadingVanHandler>();
            services.AddSingleton<RunCompletedHandler>();
            services.AddSingleton<RunStartedHandler>();
            // Add hosted service for rabbit
            services.AddHostedService<RabbitBackgroundService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {            
            app.UseMvc();
        }

        private void CreateSingletonConfigSettings<T>(IServiceCollection services, string section) where T : class, new()
        {
            services.Configure<T>(Configuration.GetSection(section));
            services.AddSingleton(s => s.GetRequiredService<IOptions<T>>().Value);
        }
    }
}