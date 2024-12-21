//-----------------------------------------------------------------
//    <copyright file="AirportList.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>21-12-2024</date>
//    <time>17:15</time>
//    <version>1.0</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// Represents a collection of airports with functionality to add, remove, and retrieve airports.
    /// Provides methods to load airports from external files and manage the fleet.
    /// </summary>
    public class AirportList
    {
        #region Private Properties

        //-----------------------------------------------------------------
        //    <summary>
        //        Dictionary that stores airports, with ICAO code as the key and Airport object as the value.
        //    </summary>
        //-----------------------------------------------------------------
        private Dictionary<string, Airport> _airports = new Dictionary<string, Airport>();

        //-----------------------------------------------------------------
        //    <summary>
        //        Logger instance to log information and errors related to airport operations.
        //    </summary>
        //-----------------------------------------------------------------
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        //-----------------------------------------------------------------
        //    <summary>
        //        Constructor to initialize the AirportList with a logger.
        //        Ensures that the logger is not null.
        //    </summary>
        //    <param name="logger">Logger instance to log information.</param>
        //-----------------------------------------------------------------
        public AirportList(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        }

        #endregion

        #region Private Methods

        //-----------------------------------------------------------------
        //    <summary>
        //        Checks whether an airport with the given ICAO code already exists in the list.
        //    </summary>
        //    <param name="registration">ICAO code of the airport to check.</param>
        //    <returns>True if the airport exists, false otherwise.</returns>
        //-----------------------------------------------------------------
        private bool DoesPlaneExist(string registration) => _airports.ContainsKey(registration);

        #endregion

        #region Public Methods

        //-----------------------------------------------------------------
        //    <summary>
        //        Adds an airport to the list if it does not already exist.
        //        Logs the addition of the airport to the fleet.
        //    </summary>
        //    <param name="airport">Airport to be added to the list.</param>
        //    <exception cref="InvalidOperationException">Thrown if the airport already exists.</exception>
        //-----------------------------------------------------------------
        public void AddAirport(Airport airport)
        {
            if (DoesPlaneExist(airport.ICAO))
            {
                throw new InvalidOperationException($"An airport with ICAO code {airport.ICAO} already exists.");
            }
            
            _airports.Add(airport.ICAO, airport);
            _logger.Info($"Airport {airport.ICAO} added to the fleet.");
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Retrieves an airport based on its ICAO code.
        //        Returns null if the airport does not exist.
        //    </summary>
        //    <param name="icao">ICAO code of the airport to retrieve.</param>
        //    <returns>Airport object if found, null otherwise.</returns>
        //-----------------------------------------------------------------
        public Airport? GetAirport(string icao)
        {
            if (_airports.TryGetValue(icao, out var airport))
            {
                return airport;
            }

            return null; // Return null if the ICAO code does not exist
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Loads airports from external files, deserializes the data,
        //        and adds the airports to the list.
        //    </summary>
        //-----------------------------------------------------------------
        public void LoadAirports()
        {
            // Retrieves a list of files containing airport data
            string[] files = FMS.Instance.ReadAirportsFromFolder();

            foreach (string file in files)
            {
                // Reads the JSON data from the file
                string json = FMS.Instance.ReadFromJson(file);

                // Deserializes the JSON data into an Airport object
                Airport airport = JsonSerializer.Deserialize<Airport>(json);

                // Adds the deserialized airport to the list
                this.AddAirport(airport);
            }
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Retrieves a list of ICAO codes from all the airports in the list.
        //    </summary>
        //    <returns>A list of ICAO codes.</returns>
        //-----------------------------------------------------------------
        public List<string> GetAirportsICAO()
        {
            List<string> airportList = new List<string>();
            foreach (Airport airport in _airports.Values)
            {
                airportList.Add(airport.ICAO);
            }
            return airportList;
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Removes an airport from the list based on its ICAO code.
        //        Also deletes the airport from the FMS system.
        //    </summary>
        //    <param name="icao">ICAO code of the airport to remove.</param>
        //-----------------------------------------------------------------
        public void RemoveAirport(string icao)
        {
            // Retrieves the airport to be removed
            Airport airport = GetAirport(icao);

            if (airport == null)
            {
                throw new InvalidOperationException($"Airport with ICAO code {icao} does not exist.");
            }

            // Deletes the airport data from the FMS system
            FMS.Instance.DeleteAirport(airport);

            // Removes the airport from the list
            _airports.Remove(icao);

            _logger.Info($"Airport {airport.ICAO} removed from the airports list.");
        }

        #endregion
    }
}
