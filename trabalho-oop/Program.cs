using System;

namespace trabalho_oop
{
    public class Program
    {
        static void Main(string[] args)
        {

            Logger logger = new Logger("./fms/logs/app.log");
            try
            {
                // Start the FMS
                
                FMS.Instance.Start(); // Start new file management system
                
                // Create SessionManager
                SessionManager sessionManager = new SessionManager(logger);

                // Load the session
                sessionManager.Load();
                sessionManager.DisplayStaff();
                
                // Display staff list
                sessionManager.DisplayPassengers();

                // Register a new staff member
                //sessionManager.RegisterPassanger("Junior", "junior@gmail.com", "junior");
                sessionManager.LoginPassenger("junior@gmail.com", "junior");
                sessionManager.IsAuthenticated();

                Passenger p = new Passenger(logger);

                if (sessionManager.GetEntityType() == EntityType.Passenger)
                {
                    p = sessionManager.ActiveSession?.LoggedInPerson as Passenger;
                }
                
                Flights flights = new Flights(logger);
                flights.LoadFlights();
                flights.ShowFlightsList();
                
                Fleet fleet = new Fleet(logger);
                fleet.LoadFleet();
                fleet.ShowAircraftList();
                // Create a new Airplane
                Airplane ryanair = new Airplane("Ryanair", "EI-GSG", 186, "Boeing 738", logger);
                FMS.Instance.Save(ryanair); // Save the Airplane instance

                //Airplane r = fleet.GetAirplane("EI-GSG");
                
                // Create a new Flight
                DateTime flightDateTime = new DateTime(2024, 12, 25, 15, 30, 0);
                Flight flight = new Flight("RYR4704", "Porto", "Milan", ryanair, logger, flightDateTime);
                flight.AddReservation(p);
                FMS.Instance.Save(flight); // Save the Flight instance
                
                // Save session data
                sessionManager.Save();
            }
            catch (Exception ex)
            {
                // Log any exceptions
                Console.WriteLine(ex);
                logger.Error($"An error occurred: {ex.Message}");
            }
        }
    }
}
