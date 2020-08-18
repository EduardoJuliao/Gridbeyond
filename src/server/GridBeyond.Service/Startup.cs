using System.Diagnostics;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Repository;
using GridBeyond.Domain.Services;
using GridBeyond.Service.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            services.AddScoped<IMarketDataRepository, MarketDataRepository>();
            services.AddTransient<IMarketDataService, MarketDataService>(provider =>
            {
                var repo = provider.GetService<IMarketDataRepository>();
                var service = new MarketDataService(repo);
                
                service.AddOnMalformedRecordEvent((sender, i) =>
                {
                    
                });

                return service;
            });
            
            services.AddDbContext<MarketContext>(opt => opt.UseInMemoryDatabase(databaseName: "GridBeyond_Market"));
            
            services.AddCors(options =>
                    {
                        options.AddPolicy(name: "MyPolicy",
                            builder =>
                            {
                                builder.WithOrigins(
                                    "https://localhost:4200",
                                    "http://localhost:5000")
                                        .WithMethods("PUT", "DELETE", "GET");
                            });
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            
            app.UseCors();
        }
    }
}
