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

            // Register the Logger as both ILogger and Logger
            builder.Services.AddSingleton<ILogger>(provider => new Logger("./fms/logs/app.log"));
            builder.Services.AddSingleton<Logger>(provider => (Logger)provider.GetRequiredService<ILogger>());
            builder.Services.AddSingleton<FMS>(provider =>
            {
                var logger = provider.GetRequiredService<Logger>();
                // If the instance hasn't been created yet, this will use the logger
                FMS.InitializeLogger(logger);
    
                // Get the singleton instance
                var fms = FMS.Instance;
    
                // Start the FMS system
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

            app.UseRouting();
            app.UseAuthorization();

            // Map API controllers
            app.MapControllers();

            // Initialize the backend and run the web app
            app.Run();
        }
    }
}