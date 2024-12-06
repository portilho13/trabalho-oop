//-----------------------------------------------------------------
//    <copyright file="Program.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System;

namespace trabalho_oop
{
    /// <summary>
    /// The main entry point for the application. It initializes various components
    /// of the Flight Management System (FMS), handles the session management,
    /// and demonstrates the functionality of different FMS entities like flights, airplanes, and passengers.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method that runs the FMS application.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            // Initialize the logger to log information, warnings, and errors
            Logger logger = new Logger("./fms/logs/app.log");
            
            try
            {
                // Start the Flight Management System (FMS) to initialize necessary folders and files
                FMS.Instance.Start(logger); // Initializes the file management system for the FMS

                // Create the session manager which manages user sessions
                SessionManager sessionManager = new SessionManager(logger);

                // Load the session data from previously saved sessions
                sessionManager.Load();
                
                // Display the list of staff members currently in the system
                sessionManager.DisplayStaff();
                
                // Display the list of passengers in the system
                sessionManager.DisplayPassengers();

                // Register and login a new passenger (commented-out for now)
                //sessionManager.RegisterPassanger("Junior", "junior@gmail.com", "junior");
                sessionManager.LoginPassenger("junior@gmail.com", "junior");
                sessionManager.IsAuthenticated();

                // Create a new passenger instance for further actions
                Passenger p = new Passenger();

                // Check if the current session is for a passenger, and assign the logged-in passenger to the `p` variable
                if (sessionManager.GetEntityType() == EntityType.Passenger)
                {
                    p = sessionManager.ActiveSession?.LoggedInPerson as Passenger;
                }
                
                // Create a new Flights instance to manage flight information
                Flights flights = new Flights(logger);
                flights.LoadFlights(); // Load flight details from storage
                flights.ShowFlightsList(); // Display the list of available flights
                
                // Create a new Fleet instance to manage aircraft
                Fleet fleet = new Fleet(logger);
                fleet.LoadFleet(); // Load fleet details from storage
                fleet.ShowAircraftList(); // Display the list of available aircraft
                
                // Create a new Airplane instance
                Airplane ryanair = new Airplane("Ryanair", "EI-GSG", 186, "Boeing 738", logger);
                FMS.Instance.Save(ryanair); // Save the airplane details to the FMS
                
                Airport Porto = new Airport("Francisco Sa Carneiro", "OPO", "LPPR", logger);
                Airport Milan = new Airport("Milan Malpensa", "MXP", "LIMC", logger);
                FMS.Instance.Save(Porto);
                FMS.Instance.Save(Milan);
                
                // Create a new Flight instance with specific details
                DateTime flightDateTime = new DateTime(2024, 12, 25, 15, 30, 0); // Set flight date and time
                Flight flight = new Flight("RYR4704", Porto, Milan, ryanair, flightDateTime, logger);
                flight.AddReservation(p); // Add the passenger reservation to the flight
                FMS.Instance.Save(flight); // Save the flight details to the FMS*/
                
                Console.WriteLine("Press any key to exit...");
                p.ShowReservations();
                
                // Save the session data (so that it can be loaded next time the system starts)
                sessionManager.Save();
            }
            catch (Exception ex)
            {
                // Handle any exceptions by logging the error and displaying it to the console
                Console.WriteLine(ex);
                logger.Error($"An error occurred: {ex.Message}");
            }
        }
    }
}