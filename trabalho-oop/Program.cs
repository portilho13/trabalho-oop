using System;

namespace trabalho_oop
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of Logger
            Logger logger = Logger.Instance("./fms/logs/app.log");

            try
            {
                // Start the FMS
                FMS fms = new FMS(); // Start new file management system
                fms.Start();
                
                // Create SessionManager
                SessionManager sessionManager = new SessionManager();
                sessionManager.fms = fms;

                // Load the session
                sessionManager.Load();
                
                // Display staff list
                sessionManager.DisplayStaffList();

                // Register a new staff member
                sessionManager.RegisterStaff("Junior", "junior.portilho2005@gmail.com", "junior");

                // Create a new Airplane
                Airplane ryanair = new Airplane("Ryanair", "EI-GSG", 186);
                fms.Save(ryanair); // Save the Airplane instance

                // Create a new Flight
                Flight flight = new Flight("RYR4703", "Porto", "Milan", ryanair, fms);
                fms.Save(flight); // Save the Flight instance
                
                // Save session data
                sessionManager.Save();
            }
            catch (Exception ex)
            {
                // Log any exceptions
                logger.Error($"An error occurred: {ex.Message}");
            }
        }
    }
}
