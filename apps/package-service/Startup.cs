using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PackageService.Handlers;
using PackageService.Service;
using PackageService.Settings;
using ShareMe.Carefully.Rabbit;

namespace PackageService
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
            CreateSingletonConfigSettings<MongoSettings>(services, "mongoSettings");
            CreateSingletonConfigSettings<RabbitSettings>(services, "rabbitSettings");

            services.AddSingleton<EventStoreService>();
            services.AddSingleton<ReadModelService>();

            services.AddSingleton(s => new RabbitHandler(s.GetRequiredService<RabbitSettings>().Uri));
            services.AddTransient<RabbitMessenger>();
            services.AddScoped<MessengerService>();

            // Setup Handlers
            services.AddSingleton<DipHandler>();
            services.AddSingleton<DncHandler>();
            services.AddSingleton<HipHandler>();
            services.AddSingleton<PackageAddedHandler>();
            services.AddSingleton<PodHandler>();
            services.AddSingleton<VipHandler>();
            services.AddSingleton<VopHandler>();
            services.AddHostedService<RabbitService>();

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