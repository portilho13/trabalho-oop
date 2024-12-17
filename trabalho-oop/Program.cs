using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace trabalho_oop
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Add CORS services and configure policies
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:3000") // Replace with your frontend's URL
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });

                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Register the Logger as both ILogger and Logger
            builder.Services.AddSingleton<ILogger>(provider => new Logger("./fms/logs/app.log"));
            builder.Services.AddSingleton<Logger>(provider => (Logger)provider.GetRequiredService<ILogger>());
            builder.Services.AddSingleton<FMS>(provider =>
            {
                var logger = provider.GetRequiredService<Logger>();
                FMS.InitializeLogger(logger);
                var fms = FMS.Instance;
                fms.Start(logger);
                return fms;
            });

            builder.Services.AddSingleton<Fleet>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger>();
                var fms = provider.GetRequiredService<FMS>();
                Fleet fleet = new Fleet(logger);

                try 
                {
                    fleet.LoadFleet();
                    logger.Info("Fleet loaded successfully");
                }
                catch (Exception ex)
                {
                    logger.Error($"Error loading fleet: {ex.Message}");
                }

                return fleet;
            });

            builder.Services.AddSingleton<AirportList>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger>();
                var fms = provider.GetRequiredService<FMS>();
                AirportList airports = new AirportList(logger);

                try 
                {
                    airports.LoadAirports();
                    logger.Info("Airports loaded successfully");
                }
                catch (Exception ex)
                {
                    logger.Error($"Error loading fleet: {ex.Message}");
                }

                return airports;
            });

            builder.Services.AddSingleton<SessionManager>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger>();
                var fms = provider.GetRequiredService<FMS>();
                SessionManager sessionManager = new SessionManager(logger);

                try 
                {
                    sessionManager.Load();
                    logger.Info("Sessions loaded successfully");
                }
                catch (Exception ex)
                {
                    logger.Error($"Error loading sessions: {ex.Message}");
                }

                return sessionManager;
            });

            var app = builder.Build();

            // Enable routing
            app.UseRouting();

            // Enable CORS middleware
            app.UseCors("AllowSpecificOrigins"); // Apply specific CORS policy

            // Enable authorization
            app.UseAuthorization();

            // Map API controllers
            app.MapControllers();

            // Initialize the backend and run the web app
            app.Run();
        }
    }
}
