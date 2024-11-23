﻿//-----------------------------------------------------------------
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
        private readonly ILogger _logger;  // Logger instance for flight-related logs

        public string Number { get; set; }  // The unique flight number (e.g., "AA123")
        public string Origin { get; private set; }  // The origin location of the flight (e.g., "New York")
        public string Destination { get; private set; }  // The destination location of the flight (e.g., "Los Angeles")
        public Airplane Airplane { get; private set; }  // The airplane assigned to the flight
        
        public DateTime ScheduledDateTime { get; private set; }  // The date and time the flight is scheduled to depart
        public Dictionary<string, Reservation> PassengersReservations { get; private set; }  // Reservations for passengers, indexed by reservation code

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
        public void AddReservation(Passenger passenger)
        {
            Reservation reservation;
            do
            {
                // Continuously generate new reservation codes until a unique one is found
                reservation = new Reservation(passenger, _logger);
            } while (PassengersReservations.ContainsKey(reservation.ReservationCode));

            // Associate the reservation with the passenger and add it to the reservations dictionary
            passenger.AddReservation(reservation);
            PassengersReservations.Add(reservation.ReservationCode, reservation);
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
        public Flight(string number, string origin, string destination, Airplane airplane, ILogger logger, DateTime scheduledDateTime)
        {
            // Ensure the logger is not null
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");

            // Validate the constructor parameters
            ValidateConstructorParameters(number, origin, destination, airplane);

            // Log flight creation details
            _logger.Info($"Creating flight {number} from {origin} to {destination} using airplane {airplane.Registration} scheduled for {scheduledDateTime}");

            // Mark the airplane as occupied (it will be used for this flight)
            airplane.ChangeOccupiedStatus();

            // Initialize flight details
            Number = number;
            Origin = origin;
            Destination = destination;
            Airplane = airplane;
            ScheduledDateTime = scheduledDateTime;  // Set the scheduled departure date and time

            // Generate a list of passenger reservations
            PassengerList p = new PassengerList(logger);
            PassengersReservations = p.GeneratePassengerList(GenerateRandomNumberOfPassengers());

            // Log successful flight creation
            _logger.Info($"Flight {number} created successfully with {PassengersReservations.Count} passengers, scheduled for {ScheduledDateTime}.");
        }
        
        /// <summary>
        /// Validates the input parameters to ensure they are not null or empty.
        /// </summary>
        /// <param name="number">The flight number.</param>
        /// <param name="origin">The origin location of the flight.</param>
        /// <param name="destination">The destination location of the flight.</param>
        /// <param name="airplane">The airplane assigned to the flight.</param>
        private void ValidateConstructorParameters(string number, string origin, string destination, Airplane airplane)
        {
            try
            {
                // Ensure the flight number is valid
                if (string.IsNullOrWhiteSpace(number))
                    throw new ArgumentException("Flight Number cannot be empty or whitespace.", nameof(number));

                // Ensure the origin is valid
                if (string.IsNullOrWhiteSpace(origin))
                    throw new ArgumentException("Origin cannot be empty or whitespace.", nameof(origin));

                // Ensure the destination is valid
                if (string.IsNullOrWhiteSpace(destination))
                    throw new ArgumentException("Destination cannot be empty or whitespace.", nameof(destination));

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
