//-----------------------------------------------------------------
//    <copyright file="Airport.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>21-12-2024</date>
//    <time>16:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// Represents an airport with properties such as airport name, IATA, and ICAO codes.
    /// Provides functionality for serialization to JSON and validation of input parameters.
    /// </summary>
    public class Airport : IStorable
    {
        #region Private Properties

        //-----------------------------------------------------------------
        //    <summary>
        //        Private logger instance to log information and errors related 
        //        to airport operations.
        //    </summary>
        //-----------------------------------------------------------------
        private readonly ILogger _logger;

        #endregion

        #region Public Properties

        //-----------------------------------------------------------------
        //    <summary>
        //        Gets or sets the name of the airport.
        //    </summary>
        //-----------------------------------------------------------------
        public string AirportName { get; set; }

        //-----------------------------------------------------------------
        //    <summary>
        //        Gets or sets the IATA code of the airport.
        //    </summary>
        //-----------------------------------------------------------------
        public string IATA { get; set; }

        //-----------------------------------------------------------------
        //    <summary>
        //        Gets or sets the ICAO code of the airport.
        //    </summary>
        //-----------------------------------------------------------------
        public string ICAO { get; set; }

        #endregion

        #region Constructors

        //-----------------------------------------------------------------
        //    <summary>
        //        Constructor for the Airport class. Initializes an airport 
        //        with the airport name, IATA, ICAO codes, and logger instance.
        //        Validates the input parameters and logs the airport creation.
        //    </summary>
        //    <param name="airportName">The name of the airport.</param>
        //    <param name="iata">The IATA code of the airport.</param>
        //    <param name="icao">The ICAO code of the airport.</param>
        //    <param name="logger">Logger instance to log information.</param>
        //-----------------------------------------------------------------
        public Airport(string airportName, string iata, string icao, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            
            // Validate the constructor parameters
            ValidateConstructorParameters(airportName, iata, icao);
            
            // Initialize properties
            AirportName = airportName;
            IATA = iata;
            ICAO = icao;
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Default constructor for deserialization. Initializes an empty instance.
        //    </summary>
        //-----------------------------------------------------------------
        public Airport() { }

        #endregion

        #region Private Methods

        //-----------------------------------------------------------------
        //    <summary>
        //        Validates the constructor parameters to ensure they meet required conditions.
        //        Throws exceptions if any of the inputs are null or empty.
        //    </summary>
        //    <param name="airportName">The name of the airport.</param>
        //    <param name="iata">The IATA code of the airport.</param>
        //    <param name="icao">The ICAO code of the airport.</param>
        //-----------------------------------------------------------------
        private void ValidateConstructorParameters(string airportName, string iata, string icao)
        {
            // Ensure airportName is not null or empty
            if (string.IsNullOrEmpty(airportName))
                throw new ArgumentNullException(nameof(airportName), "Airport Name cannot be null or empty.");
            
            // Ensure IATA code is not null or empty
            if (string.IsNullOrEmpty(iata))
                throw new ArgumentNullException(nameof(iata), "IATA cannot be null or empty.");
            
            // Ensure ICAO code is not null or empty
            if (string.IsNullOrEmpty(icao))
                throw new ArgumentNullException(nameof(icao), "ICAO cannot be null or empty.");
        }

        #endregion

        #region Public Methods

        //-----------------------------------------------------------------
        //    <summary>
        //        Returns the entity type for this object, which is Airport.
        //    </summary>
        //    <returns>The entity type of the object (Airport).</returns>
        //-----------------------------------------------------------------
        public EntityType GetEntityType() => EntityType.Airport;

        //-----------------------------------------------------------------
        //    <summary>
        //        Converts the airport object to a formatted JSON string for storage.
        //    </summary>
        //    <returns>A JSON string representing the airport object.</returns>
        //-----------------------------------------------------------------
        public string ConvertToJson()
        {
            try
            {
                // Serialize the object to a formatted JSON string
                return JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            }
            catch (JsonException ex)
            {
                // Log the JSON serialization error and throw a detailed exception
                throw new JsonException($"Failed to serialize airport {AirportName} to JSON", ex);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors during serialization
                throw new InvalidOperationException($"Unexpected error while converting airport {AirportName} to JSON", ex);
            }
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Returns the identifier of the airport, which is its ICAO code.
        //    </summary>
        //    <returns>The ICAO code of the airport.</returns>
        //-----------------------------------------------------------------
        public string GetIdentifier() => ICAO;

        #endregion
    }
}
