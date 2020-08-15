using System.Diagnostics;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Repository;
using GridBeyond.Domain.Services;
using GridBeyond.Service.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GridBeyond.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();

            services.AddSingleton<IMarketDataHub, MarketDataHub>();
            services.AddSingleton<IMarketDataRepository, MarketDataRepository>();
            services.AddTransient<IMarketDataService, MarketDataService>(provider =>
            {
                var repo = provider.GetService<IMarketDataRepository>();
                var service = new MarketDataService(repo);
                
                service.AddOnMalformedRecordEvent((sender, i) =>
                {
                    
                });

                return service;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            , IMarketDataService marketService, IMarketDataHub mdhub)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MarketDataHub>("/mdhub");
            });
        }
    }
}
