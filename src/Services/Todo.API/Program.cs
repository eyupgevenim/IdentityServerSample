namespace Todo.API
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using System;
    using System.IO;
    using Todo.API.Data;
    using Todo.API.Data.Seeds;
    using Todo.API.Infrastructure.Exceptions;

    public class Program
    {
        readonly static string Namespace = typeof(Startup).Namespace;
        readonly static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
                var host = BuildWebHost(configuration, args);

                Log.Information("Applying migrations ({ApplicationContext})...", AppName);
                host.MigrateDbContext<TodoDbContext>((context, services) =>
                {
                    var logger = services.GetService<ILogger<TodoDbContextSeed>>();
                    new TodoDbContextSeed().SeedAsync(context, logger).Wait();
                });

                Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static IWebHost BuildWebHost(IConfiguration configuration, string[] args) => 
            WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(false)
            //.ConfigureKestrel(options =>
            //{
            //    var ports = GetDefinedPorts(configuration);
            //    options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
            //    {
            //        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            //    });

            //    options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
            //    {
            //        listenOptions.Protocols = HttpProtocols.Http2;
            //    });
            //})
            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
            .UseStartup<Startup>()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseSerilog()
            .Build();

        static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", Program.AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 5001);
            var port = config.GetValue("PORT", 80);
            return (port, grpcPort);
        }

    }
}
