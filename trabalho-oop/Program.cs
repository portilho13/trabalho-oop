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

                FMS.Instance.Start(); // Start new file management system
                
                // Create SessionManager
                SessionManager sessionManager = new SessionManager();
                sessionManager.fms = FMS.Instance;

                // Load the session
                sessionManager.Load();
                
                // Display staff list
                sessionManager.DisplayStaffList();

                // Register a new staff member
                sessionManager.RegisterPassanger("Junior", "junior.portilho2005@gmail.com", "junior");
                sessionManager.LoginPassenger("junior.portilho2005@gmail.com", "junior");
                sessionManager.IsAuthenticated();

                Passenger p = new Passenger();

                if (sessionManager.GetEntityType() == EntityType.Passenger)
                {
                    p = sessionManager.ActiveSession?.LoggedInPerson as Passenger;
                }
                
                Fleet fleet = new Fleet();
                fleet.LoadFleet();
                fleet.ShowAircraftList();
                // Create a new Airplane
                Airplane ryanair = new Airplane("Ryanair", "EI-GSG", 186, "Boeing 738");
                //fms.Save(ryanair); // Save the Airplane instance
                
                // Create a new Flight
                //Flight flight = new Flight("RYR4703", "Porto", "Milan", ryanair, fms);
                //flight.AddReservation(p);
                //FMS.Instance.Save(flight); // Save the Flight instance
                
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
