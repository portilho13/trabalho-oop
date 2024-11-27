//-----------------------------------------------------------------
//    <copyright file="Flights.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// Manages a collection of flights, allowing for adding, removing, loading, and displaying flights.
    /// </summary>
    public class Flights
    {
        // Dictionary to store flights with the flight number as the key and Flight object as the value
        private Dictionary<string, Flight> _flights = new Dictionary<string, Flight>();

        // Logger instance to log actions performed on the flights
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for initializing the Flights collection with a logger.
        /// </summary>
        /// <param name="logger">Logger instance used to log actions.</param>
        public Flights(ILogger logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Checks if a flight with the given flight number already exists in the collection.
        /// </summary>
        /// <param name="flightNumber">The flight number to check.</param>
        /// <returns>True if the flight exists, otherwise false.</returns>
        private bool DoesFlightExist(string flightNumber) => _flights.ContainsKey(flightNumber);

        /// <summary>
        /// Adds a new flight to the collection if it doesn't already exist.
        /// </summary>
        /// <param name="flight">The flight object to be added.</param>
        /// <exception cref="InvalidOperationException">Thrown if a flight with the same number already exists.</exception>
        public void AddFlight(Flight flight)
        {
            if (DoesFlightExist(flight.Number))
            {
                throw new InvalidOperationException($"A flight with number {flight.Number} already exists.");
            }

            // Adds the flight to the collection and logs the addition
            _flights.Add(flight.Number, flight);
            _logger.Info($"Flight {flight.Number} added to flights.");
        }

        /// <summary>
        /// Loads flights from files, deserializes them, and adds them to the collection.
        /// </summary>
        public void LoadFlights()
        {
            // Retrieves a list of files containing flight data
            string[] files = FMS.Instance.ReadFlightsFromFolder();

            // Loops through each file, deserializes the content, and adds the flight to the collection
            foreach (string file in files)
            {
                // Reads the JSON data from the file
                string json = FMS.Instance.ReadFromJson(file);

                // Deserializes the JSON data into a Flight object
                Flight flight = JsonSerializer.Deserialize<Flight>(json);

                // Adds the deserialized flight to the collection
                this.AddFlight(flight);
            }
        }

        /// <summary>
        /// Retrieves a flight by its flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to retrieve.</param>
        /// <returns>The Flight object associated with the provided flight number.</returns>
        public Flight GetFlight(string flightNumber)
        {
            return _flights[flightNumber];
        }

        /// <summary>
        /// Deletes a flight by its flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to delete.</param>
        public void DeleteFlight(string flightNumber)
        {
            // Retrieves the flight by its flight number
            Flight flight = GetFlight(flightNumber);

            // Deletes the flight data from the file system
            FMS.Instance.DeleteFlight(flight);

            // Removes the flight from the collection
            _flights.Remove(flightNumber);

            // Logs the deletion action
            _logger.Info($"Flight {flight.Number} deleted from flights.");
        }

        /// <summary>
        /// Displays the list of all flight numbers currently in the collection.
        /// </summary>
        public void ShowFlightsList()
        {
            // Loops through each flight and prints its number to the console
            foreach (Flight flight in _flights.Values)
            {
                Console.WriteLine(flight.Number);
            }
        }
    }
}