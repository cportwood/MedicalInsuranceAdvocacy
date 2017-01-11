using System.Linq;
using AutoMapper;
using MedicalInsuranceAdvocacy.DbContext;
using MedicalInsuranceAdvocacy.DbContext.DbContexts;
using MedicalInsuranceAdvocacy.Service;
using MedicalInsuranceAdvocacy.WebApi.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Reflection;

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
               // builder.AddApplicationInsightsSettings(developerMode: true);
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
            var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            // Add framework services.
            services.AddMemoryCache();
            //services.AddApplicationInsightsTelemetry(Configuration);
            //Using In Memory Database
            services.AddDbContext<AspIdentityDbContext>(options => options.UseSqlServer(connectionString,b => b.MigrationsAssembly(migrationsAssembly))).AddDbContext<AspIdentityDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AspIdentityDbContext>();
            services.AddIdentityServer()
            .AddOperationalStore(
                builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
            //.AddInMemoryClients(Clients.Get())
            //.AddInMemoryIdentityResources(Resources.GetIdentityResources())
            //.AddInMemoryApiResources(Resources.GetApiResources())
            .AddConfigurationStore(
                builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
            //.AddInMemoryUsers(Users.Get())
            .AddAspNetIdentity<IdentityUser>()
            .AddTemporarySigningCredential();
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
            //app.UseIdentityServer();

            //Seeding Data
            var context = app.ApplicationServices.GetService<ApplicationContext>();
            DatabaseSeeder.SeedPatientData(context);
            app.UseDeveloperExceptionPage();

            InitializeDbTestData(app);

            app.UseIdentity();
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

        }
        private static void InitializeDbTestData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<AspIdentityDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!context.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var inMemoryUser in Users.Get())
                    {
                        var identityUser = new IdentityUser(inMemoryUser.Username)
                        {
                            Id = inMemoryUser.SubjectId
                        };

                        foreach (var claim in inMemoryUser.Claims)
                        {
                            identityUser.Claims.Add(new IdentityUserClaim<string>
                            {
                                UserId = identityUser.Id,
                                ClaimType = claim.Type,
                                ClaimValue = claim.Value,
                            });
                        }

                        userManager.CreateAsync(identityUser, "Password123!").Wait();
                    }
                }
            }
        }
    }
}
