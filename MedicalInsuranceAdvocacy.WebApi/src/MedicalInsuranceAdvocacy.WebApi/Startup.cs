using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Exl.WebApi.Aspen;
using MedicalInsuranceAdvocacy.DbContext;
using MedicalInsuranceAdvocacy.DbContext.DbContexts;
using MedicalInsuranceAdvocacy.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace MedicalInsuranceAdvocacy.WebApi
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            //AutoMapper Config
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
                //MapperRegistry.Mappers.Add(new DataReaderMapper { YieldReturnEnabled = true });
            });
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMemoryCache();
            services.AddApplicationInsightsTelemetry(Configuration);
            //Using In Memory Database
            services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase());
            //services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddMvc();
            //Automapper Config
            services.AddSingleton(sp => _mapperConfiguration.CreateMapper());
            services.RegisterServices();
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            //Seeding Data
            var context = app.ApplicationServices.GetService<ApplicationContext>();
            DatabaseSeeder.SeedPatientData(context);

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvcWithDefaultRoute();
        }
    }
}
