using KnowledgeSpace.BackendServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace KnowledgeSpace.BackendServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();

            // === Modify use add Seeding data ===
            var hostBuilder = CreateHostBuilder(args).Build();

            // -- Serilog - it doesn't seem to work with latest net 5 --
            Log.Logger = new LoggerConfiguration()
                             .Enrich.FromLogContext()
                             // .WriteTo.Console() // it doesn't seem to work with latest net 5
                             // .WriteTo.File("log.txt") // it doesn't seem to work with latest net 5
                             .CreateLogger();
            // -- Seed data --
            // DependencyInjection
            using (var scope = hostBuilder.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    Log.Information("Seeding data...");
                    var dbInitializer = services.GetService<DbInitializer>();
                    dbInitializer.Seed().Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            // -- /.Seed data --

            hostBuilder.Run();

            // === /.Modify use add Seeding data ===
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)) // Hook Serilog into the Web API
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    // --- IIS ---
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIISIntegration();
                    // --- /.IIS ---
                });
    }
}