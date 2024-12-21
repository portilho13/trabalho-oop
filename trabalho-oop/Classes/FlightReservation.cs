//-----------------------------------------------------------------
//    <copyright file="FlightReservation.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>21-12-2024</date>
//    <time>17:15</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

namespace trabalho_oop
{
    /// <summary>
    /// Represents a flight reservation, which is a type of reservation specific to flights.
    /// It extends from the base Reservation class and includes additional information such as the passenger's name.
    /// </summary>
    public class FlightReservation : Reservation
    {
        /// <summary>
        /// Gets or sets the name of the passenger who made the reservation.
        /// </summary>
        public string PassengerName { get; set; }
        
        /// <summary>
        /// Default constructor for FlightReservation.
        /// Initializes a new instance of the FlightReservation class.
        /// </summary>
        public FlightReservation() { }

        /// <summary>
        /// Initializes a new instance of the FlightReservation class with specified reservation code and passenger name.
        /// </summary>
        /// <param name="reservationCode">The unique reservation code for the flight reservation.</param>
        /// <param name="passengerName">The name of the passenger who made the reservation.</param>
        public FlightReservation(string reservationCode, string passengerName)
        {
            ReservationCode = reservationCode;  // Set the reservation code from the base class
            PassengerName = passengerName;  // Set the passenger's name
        }
    }
}