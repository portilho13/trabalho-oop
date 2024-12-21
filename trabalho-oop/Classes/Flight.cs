//-----------------------------------------------------------------
//    <copyright file="Flight.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace trabalho_oop
{
    /// <summary>
    /// Represents a flight in the system, including flight details, reservations, and the associated airplane.
    /// </summary>
    public class Flight : IStorable
    {
        public string Number { get; set; }  // The unique flight number (e.g., "AA123")
        public Airport Origin { get; private set; }  // The origin location of the flight (e.g., "New York")
        public Airport Destination { get; private set; }  // The destination location of the flight (e.g., "Los Angeles")
        public Airplane Airplane { get; private set; }  // The airplane assigned to the flight

        private readonly ILogger _logger;
        public DateTime ScheduledDateTime { get; private set; }  // The date and time the flight is scheduled to depart

        public Dictionary<string, FlightReservation> PassengersReservations { get; private set; } =
            new Dictionary<string, FlightReservation>(); // Reservations for passengers, indexed by reservation code

        /// <summary>
        /// Gets the unique identifier for the flight, which is the flight number.
        /// </summary>
        public string GetIdentifier() => Number;

        /// <summary>
        /// Converts the flight object to a JSON string for storage or transmission.
        /// </summary>
        /// <returns>A JSON string representing the flight.</returns>
        public string ConvertToJson()
        {
            try
            {
                // Serialize the flight object to JSON format with indentation for readability
                return JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            }
            catch (JsonException ex)
            {
                // Log and throw an exception if JSON serialization fails
                throw new JsonException($"Failed to serialize flight {Number} to JSON", ex);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected exceptions and throw an InvalidOperationException
                throw new InvalidOperationException($"Unexpected error while converting airplane {Number} to JSON", ex);
            }
        }

        /// <summary>
        /// Gets the entity type for this object in the FMS system, which is a Flight.
        /// </summary>
        /// <returns>EntityType.Flight</returns>
        public EntityType GetEntityType() => EntityType.Flight;

        /// <summary>
        /// Generates a random number of passengers for the flight, between 100 and the airplane's capacity.
        /// </summary>
        /// <returns>A random number of passengers between 100 and the airplane's capacity.</returns>
        private int GenerateRandomNumberOfPassengers()
        {
            Random random = new Random();
            return random.Next(100, Airplane.Capacity); // Random value between 100 and Airplane's max capacity
        }
        
        /// <summary>
        /// Checks if a reservation exists for a given reservation code.
        /// Throws an exception if the reservation is not found.
        /// </summary>
        /// <param name="reservationCode">The reservation code to check.</param>
        public void CheckReservationExists(string reservationCode)
        {
            if (!PassengersReservations.ContainsKey(reservationCode))
            {
                throw new KeyNotFoundException($"Reservation with code {reservationCode} does not exist.");
            }
        }

        /// <summary>
        /// Retrieves a reservation based on a reservation code.
        /// Throws an exception if the reservation does not exist.
        /// </summary>
        /// <param name="reservationCode">The reservation code to retrieve.</param>
        /// <returns>The reservation corresponding to the given reservation code.</returns>
        public Reservation GetReservation(string reservationCode)
        {
            CheckReservationExists(reservationCode); // Ensure the reservation exists
            return PassengersReservations[reservationCode];
        }


        /// <summary>
        /// Adds a new reservation for a passenger, ensuring each reservation code is unique.
        /// </summary>
        /// <param name="passenger">The passenger to add a reservation for.</param>
        public string AddReservation(string name)
        {
            FlightReservation flightReservation;
            string reservationCode;

            // Generate a unique reservation code
            do
            {
                reservationCode = NumberGenerator.GenerateRandomNumber();
            } while (PassengersReservations.ContainsKey(reservationCode));
            

            flightReservation = new FlightReservation
            {
                ReservationCode = reservationCode,
                PassengerName = name
            };

            // Associate the reservation with the passenger and add it to the dictionary
            PassengersReservations.Add(reservationCode, flightReservation);
            return reservationCode;
        }

        
        public Flight() {} // Constructor for deserelization 
        
        /// <summary>
        /// Initializes a new flight with the provided details.
        /// </summary>
        /// <param name="number">The flight number (e.g., "AA123").</param>
        /// <param name="origin">The origin location of the flight.</param>
        /// <param name="destination">The destination location of the flight.</param>
        /// <param name="airplane">The airplane assigned to the flight.</param>
        /// <param name="logger">A logger instance for logging flight-related activities.</param>
        /// <param name="scheduledDateTime">The scheduled date and time for the flight.</param>
        public Flight(string number, Airport origin, Airport destination, Airplane airplane, DateTime scheduledDateTime, ILogger logger)
        {
            // Ensure the logger is not null
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

            // Validate the constructor parameters
            ValidateConstructorParameters(number, origin, destination, airplane);
            

            // Mark the airplane as occupied (it will be used for this flight)
            airplane.ChangeOccupiedStatus();

            // Initialize flight details
            Number = number;
            Origin = origin;
            Destination = destination;
            Airplane = airplane;
            ScheduledDateTime = scheduledDateTime;  // Set the scheduled departure date and time

            // Generate a list of passenger reservations
            PassengerList p = new PassengerList();
            PassengersReservations = p.GeneratePassengerList(GenerateRandomNumberOfPassengers());
            
        }
        
        /// <summary>
        /// Validates the input parameters to ensure they are not null or empty.
        /// </summary>
        /// <param name="number">The flight number.</param>
        /// <param name="origin">The origin location of the flight.</param>
        /// <param name="destination">The destination location of the flight.</param>
        /// <param name="airplane">The airplane assigned to the flight.</param>
        private void ValidateConstructorParameters(string number, Airport origin, Airport destination, Airplane airplane)
        {
            try
            {
                // Ensure the flight number is valid
                if (string.IsNullOrWhiteSpace(number))
                    throw new ArgumentException("Flight Number cannot be empty or whitespace.", nameof(number));

                // Ensure the origin is valid
                if (origin == null)
                    throw new ArgumentException("Origin cannot be null.", nameof(origin));

                // Ensure the destination is valid
                if (destination == null)
                    throw new ArgumentException("Destination cannot be null.", nameof(destination));

                // Ensure the airplane is not null
                if (airplane == null)
                    throw new ArgumentNullException(nameof(airplane), "Airplane cannot be null.");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentOutOfRangeException)
            {
                // Rethrow validation errors to be handled at a higher level
                throw;
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors during validation
                throw new InvalidOperationException("Unexpected error during parameter validation", ex);
            }
        }
        
    }
}