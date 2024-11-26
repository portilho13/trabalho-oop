//-----------------------------------------------------------------
//    <copyright file="Reservation.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System;
using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// The Reservation class represents a reservation made by a passenger for a flight.
    /// It contains reservation details like the reservation code and the passenger information.
    /// </summary>
    public class Reservation
    {
        private readonly ILogger _logger;

        // The reservation code (unique identifier), will be generated automatically
        [NonSerialized] public string ReservationCode;

        // The passenger associated with the reservation
        public Person Passenger { get; set; }

        /// <summary>
        /// Constructor for creating a new reservation.
        /// The reservation code is generated automatically.
        /// </summary>
        /// <param name="passenger">The passenger associated with the reservation.</param>
        /// <param name="logger">Logger for logging activities related to this reservation.</param>
        public Reservation(Passenger passenger, ILogger logger)
        {
            // Set the logger for logging reservation activities
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            
            // Validate the input parameters for the constructor
            ValidateConstructorParameters(passenger);
            
            // Set the passenger and generate the reservation code
            Passenger = passenger;
            ReservationCode = NumberGenerator.GenerateRandomNumber();
            
        }

        /// <summary>
        /// Validates the parameters passed to the constructor to ensure valid values are provided.
        /// Throws an exception if any parameter is invalid.
        /// </summary>
        /// <param name="passenger">The passenger to be associated with the reservation.</param>
        private void ValidateConstructorParameters(Passenger passenger)
        {
            // Check if the passenger is null
            if (passenger == null)
            {
                throw new ArgumentNullException(nameof(passenger), "Passenger cannot be null.");
            }
        }

        /// <summary>
        /// Gets the unique identifier for this reservation, which is the reservation code.
        /// </summary>
        /// <returns>The reservation code.</returns>
        public string GetIdentifier() => ReservationCode;
    }
}
