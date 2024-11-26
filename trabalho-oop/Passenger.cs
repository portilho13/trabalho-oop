//-----------------------------------------------------------------
//    <copyright file="Passenger.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// Represents a passenger who can make reservations and be serialized to JSON.
    /// Inherits from Person and implements the IStorable interface.
    /// </summary>
    public class Passenger : Person, IStorable
    {
        // Fields
        public string Password { get; set; }
        public string Id { get; set; }
        
        // Logger instance to log actions performed by the passenger
        [NonSerialized] // Prevents the logger from being serialized
        private readonly ILogger _logger;
        
        // Collection of reservations made by the passenger
        public Dictionary<string, Reservation> Reservations { get; set; } = new Dictionary<string, Reservation>();

        /// <summary>
        /// Constructor to create a new passenger with a unique identifier.
        /// Initializes the logger instance.
        /// </summary>
        /// <param name="logger">The logger instance used for logging passenger actions.</param>
        public Passenger()
        {
            Id = NumberGenerator.GenerateRandomNumber(); // Generate a unique passenger ID
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        }

        /// <summary>
        /// Parameterless constructor needed for deserialization (e.g., JSON deserialization).
        /// </summary>
        public Passenger()
        {
        }

        /// <summary>
        /// Returns the unique identifier for the passenger.
        /// </summary>
        /// <returns>The passenger's unique ID.</returns>
        public string GetIdentifier() => Id;

        /// <summary>
        /// Converts the passenger object to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the passenger.</returns>
        public string ConvertToJson()
        {
            try
            {
                // Serialize the passenger object to a pretty-formatted JSON string
                return JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            }
            catch (JsonException ex)
            {
                // Handles JSON serialization errors
                throw new JsonException($"Failed to serialize passenger {Name} to JSON", ex);
            }
            catch (Exception ex)
            {
                // Handles unexpected errors during serialization
                throw new InvalidOperationException($"Unexpected error while converting passenger {Name} to JSON", ex);
            }
        }

        /// <summary>
        /// Returns the entity type of the passenger (to be used in generic data handling).
        /// </summary>
        /// <returns>The entity type for a passenger.</returns>
        public EntityType GetEntityType() => EntityType.Passenger;

        // Private helper method to check if a reservation already exists
        private bool DoesReservationExists(string reservationCode) => Reservations.ContainsKey(reservationCode);

        /// <summary>
        /// Adds a reservation to the passenger's collection if it doesn't already exist.
        /// </summary>
        /// <param name="reservation">The reservation to add.</param>
        public void AddReservation(Reservation reservation)
        {
            // Check if the reservation already exists
            if (!DoesReservationExists(reservation.ReservationCode))
                // Add the reservation if it doesn't exist
                Reservations.Add(reservation.ReservationCode, reservation);
                _logger.Info($"Added reservation {reservation.ReservationCode} for passenger {Id}.");

        }
    }
}
