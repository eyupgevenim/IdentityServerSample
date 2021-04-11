using HealthChecks.UI.Client;
using Identity.API.Data;
using Identity.API.Data.Entities.Identity;
using Identity.API.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Identity.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomApplicationInsights(Configuration)
                .AddCustomDatabaseConfiguration(Configuration)
                .AddCustomHealthChecks(Configuration)
                .AddCustomServices(Configuration)
                .AddCustomIdentityServer(Configuration)
                .AddCustomMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            app
                .UseCustomHandler(env)
                .UseCustomLogger(loggerFactory, Configuration)
                .UseStaticFiles()
                // Make work identity server redirections in Edge and lastest versions of browers. WARN: Not valid in a production environment.
                .UseCustomContentSecurityPolicy()
                .UseForwardedHeaders()
                // Adds IdentityServer
                .UseIdentityServer()
                .UseCustomCookiePolicy()
                .UseRouting()
                .UseAuthorization()
                .UseCustomEndpoints();
        }

    }

    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            return services;
        }

        public static IServiceCollection AddCustomDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var connectionString = configuration["ConnectionString"];

            Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = (sqlOptions) =>
            {
                sqlOptions.MigrationsAssembly(migrationsAssembly);
                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            };

            // Add framework services.
            services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlServerOptionsAction));
            services.AddDbContext<ApplicationPersistedGrantDbContext>(options => options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlServerOptionsAction));
            services.AddDbContext<ApplicationConfigurationDbContext>(options => options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlServerOptionsAction));

            return services;
        }

        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(configuration["ConnectionString"], name: "IdentityDB-check", tags: new string[] { "IdentityDB" });

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            services.AddTransient<ILoginService<User>, EFLoginService>();

            return services;
        }

        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var connectionString = configuration["ConnectionString"];

            Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = (sqlOptions) =>
            {
                sqlOptions.MigrationsAssembly(migrationsAssembly);
                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            };

            // Adds IdentityServer
            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters = "abcçdefghiıjklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._@+'#!/^%{}*";
            }).AddEntityFrameworkStores<ApplicationIdentityDbContext>()
              .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {
                options.IssuerUri = "null";
                options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
            })
            .AddDeveloperSigningCredential()//.AddSigningCredential(Certificate.Get())
            .AddOperationalStore(options =>
            {
                options.EnableTokenCleanup = true;
                options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlServerOptionsAction);
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlServerOptionsAction);
            })
            .AddAspNetIdentity<User>()
            .Services.AddTransient<IProfileService, ProfileService>();

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddRazorPages();

            return services;
        }

    }

    static class ConfigureExtensions
    {
        public static IApplicationBuilder UseCustomHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            return app;
        }

        public static IApplicationBuilder UseCustomLogger(this IApplicationBuilder app, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            var pathBase = configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            return app;
        }

        public static IApplicationBuilder UseCustomContentSecurityPolicy(this IApplicationBuilder app)
        {
            // Make work identity server redirections in Edge and lastest versions of browers. WARN: Not valid in a production environment.
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "script-src 'unsafe-inline'");
                await next();
            });

            return app;
        }

        public static IApplicationBuilder UseCustomCookiePolicy(this IApplicationBuilder app)
        {
            // Fix a problem with chrome. Chrome enabled a new feature "Cookies without SameSite must be secure", 
            // the coockies shold be expided from https, but in eShop, the internal comunicacion in aks and docker compose is http.
            // To avoid this problem, the policy of cookies shold be in Lax mode.
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax });

            return app;
        }

        public static IApplicationBuilder UseCustomEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });

            return app;
        }
    }
}
