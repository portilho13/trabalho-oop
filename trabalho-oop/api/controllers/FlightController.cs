//-----------------------------------------------------------------
//    <copyright file="FlightController.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>21-12-2024</date>
//    <time>17:15</time>
//    <version>1.0</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using trabalho_oop.api.models;

namespace trabalho_oop.api.controllers
{
    #region Class Documentation

    /// <summary>
    /// The FlightController class provides API endpoints for managing flights.
    /// It allows adding, retrieving, and deleting flights, as well as adding reservations to flights.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        #endregion

        #region Fields

        // The list of flights to manage
        private readonly Flights _flightList;
        // Logger for capturing logs
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FlightController class.
        /// </summary>
        /// <param name="flights">The list of flights that the controller will manage.</param>
        /// <param name="logger">The logger used to log flight-related activities.</param>
        public FlightController(Flights flights, ILogger logger)
        {
            _flightList = flights ?? throw new ArgumentNullException(nameof(flights), "Flight list is null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger is null.");
        }

        #endregion

        #region Flight Management

        /// <summary>
        /// Retrieves the list of all flights.
        /// </summary>
        /// <returns>A list of flights.</returns>
        [HttpGet]
        public IActionResult GetFlights()
        {
            try
            {
                List<Flight> flightList = new List<Flight>();
                var flights = _flightList.GetFlights();
                foreach (var flight in flights)
                {
                    Flight f = _flightList.GetFlight(flight);
                    flightList.Add(f);
                }
                return Ok(flightList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving flights.", error = ex.Message });
            }
        }

        /// <summary>
        /// Adds a new flight to the system.
        /// </summary>
        /// <param name="flight">The flight details to add.</param>
        /// <returns>A response indicating the success or failure of the flight addition.</returns>
        [HttpPost]
        public IActionResult AddFlight([FromBody] models.Flight flight)
        {
            try
            {
                // Create origin and destination airports
                Airport origin = new Airport(flight.Origin.Name, flight.Origin.IATA, flight.Origin.ICAO, _logger);
                Airport destination = new Airport(flight.Destination.Name, flight.Destination.IATA, flight.Destination.ICAO, _logger);

                // Create airplane instance
                Airplane airplane = new Airplane(flight.Airplane.Company, flight.Airplane.Registration, flight.Airplane.Capacity,
                    flight.Airplane.Model, _logger);

                // Create flight instance
                Flight f = new Flight(flight.Number, origin, destination, airplane, flight.ScheduledDateTime, _logger);

                // Add the flight to the flight list and save it
                _flightList.AddFlight(f);
                FMS.Instance.Save(f);

                return Ok(new { message = $"Flight {flight.Number} added successfully." });
            }
            catch (Exception ex)
            {
                // Return internal server error if an exception occurs
                return StatusCode(500, new { message = "An error occurred while adding the flight.", error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a flight from the system by its flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to delete.</param>
        /// <returns>A response indicating the success of the flight deletion.</returns>
        [HttpDelete("{flightNumber}")]
        public IActionResult DeleteFlight(string flightNumber)
        {
            try
            {
                _flightList.DeleteFlight(flightNumber);
                return Ok(new { message = $"Flight {flightNumber} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the flight.", error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves details of a specific flight by its flight number.
        /// </summary>
        /// <param name="flightNumber">The flight number of the flight to retrieve.</param>
        /// <returns>The flight details.</returns>
        [HttpGet("{flightNumber}")]
        public IActionResult GetFlight(string flightNumber)
        {
            try
            {
                var flight = _flightList.GetFlight(flightNumber);
                if (flight == null)
                {
                    return NotFound(new { message = $"Flight {flightNumber} not found." });
                }

                return Ok(flight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the flight.", error = ex.Message });
            }
        }

        #endregion

        #region Reservation Management

        /// <summary>
        /// Adds a reservation to a flight.
        /// </summary>
        /// <param name="reservation">The reservation details to add.</param>
        /// <param name="flightNumber">The flight number for which the reservation is being made.</param>
        /// <returns>A response containing the reservation code.</returns>
        [HttpPost("{flightNumber}")]
        public IActionResult AddReservation([FromBody] models.ReservationDetails reservation, string flightNumber)
        {
            try
            {
                // Retrieve the flight using the flight number
                Flight flight = _flightList.GetFlight(flightNumber);

                if (flight == null)
                {
                    return NotFound(new { message = $"Flight {flightNumber} not found." });
                }

                // Add the reservation to the flight
                string reservationCode = flight.AddReservation(reservation.Name);

                return Ok(new { message = $"Reservation added successfully. Reservation code: {reservationCode}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the reservation.", error = ex.Message });
            }
        }

        #endregion
    }
}
