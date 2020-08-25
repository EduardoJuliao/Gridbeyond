using System;
using System.Diagnostics;
using System.IO;
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
            services.AddCors();
            services.AddMvc();
            services.AddControllers();
            services.AddSignalR();

            services.AddTransient<IMarketDataRepository, MarketDataRepository>();
            services.AddTransient<IProcessHistoryRepository, ProcessHistoryRepository>();

            services.AddSingleton<IMarketDataHub, MarketDataHub>();

            services.AddSingleton<IProcessHistoryService, ProcessHistoryService>();
            services.AddSingleton<ICacheRepository, CacheRepository>();
            services.AddTransient<IMarketDataService, MarketDataService>(provider =>
            {
                var repo = provider.GetService<IMarketDataRepository>();
                var cache = provider.GetService<ICacheRepository>();
                var service = new MarketDataService(repo, cache);

                service.AddOnMalformedRecordEvent((sender, i) => { });

                return service;
            });

            var databaseMode = Configuration.GetSection("Database").GetValue<DatabaseMode>("Mode");

            if (databaseMode == DatabaseMode.MEMORY)
            {
                services.AddDbContext<MarketContext>(opt =>
                    opt.UseInMemoryDatabase(databaseName: "GridBeyond_Market"), ServiceLifetime.Singleton);
            }
            else if (databaseMode == DatabaseMode.SERVER)
            {
                var connectionString = Configuration.GetSection("Database").GetValue<string>("ConnectionString");
                services.AddDbContext<MarketContext>(opt =>
                    opt.UseSqlServer(connectionString), ServiceLifetime.Singleton);
            }
            else
            {
                throw new System.Exception("Invalid configuration file. Missing Database:Mode");
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
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