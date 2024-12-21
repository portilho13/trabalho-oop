//-----------------------------------------------------------------
//    <copyright file="SessionManagerController.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>21-12-2024</date>
//    <time>17:15</time>
//    <version>1.0</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using trabalho_oop.api.models;

namespace trabalho_oop.api.controllers
{
    #region Class Documentation

    /// <summary>
    /// The SessionManagerController class provides API endpoints to handle user sessions.
    /// It supports actions for login, registration, logout, authentication status, and managing reservations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SessionManagerController : ControllerBase
    {
        #endregion

        #region Fields

        // The session manager instance that handles user session logic
        private readonly SessionManager _sessionManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SessionManagerController class.
        /// </summary>
        /// <param name="sessionManager">The session manager instance used by the controller to manage user sessions.</param>
        public SessionManagerController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager), "Session manager is null");
        }

        #endregion

        #region Login Methods

        /// <summary>
        /// Logs in a staff member using their credentials.
        /// </summary>
        /// <param name="request">The login request containing the staff member's email and password.</param>
        /// <returns>A response indicating the login status, including a message and user details if successful.</returns>
        [HttpPost("Login/Staff")]
        public IActionResult LoginStaff([FromBody] LoginRequest request)
        {
            try
            {
                var result = _sessionManager.LoginStaff(request.Email, request.Password);
                if (result)
                {
                    return Ok(new { message = "Login successful", user = _sessionManager.GetLoggedInPerson() });
                }
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Logs in a passenger using their credentials.
        /// </summary>
        /// <param name="request">The login request containing the passenger's email and password.</param>
        /// <returns>A response indicating the login status, including a message and user details if successful.</returns>
        [HttpPost("Login/Passenger")]
        public IActionResult LoginPassenger([FromBody] LoginRequest request)
        {
            try
            {
                bool result = _sessionManager.LoginPassenger(request.Email, request.Password);
                if (result)
                {
                    return Ok(new { message = "Login successful", user = _sessionManager.GetLoggedInPerson() });
                }
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Register Methods

        /// <summary>
        /// Registers a new staff member.
        /// </summary>
        /// <param name="request">The registration request containing the staff member's name, email, and password.</param>
        /// <returns>A response indicating the registration status.</returns>
        [HttpPost("Register/Staff")]
        public IActionResult RegisterStaff([FromBody] RegisterRequest request)
        {
            try
            {
                _sessionManager.RegisterStaff(request.Name, request.Email, request.Password);
                _sessionManager.Save();
                return Ok(new { message = "Staff registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Registers a new passenger.
        /// </summary>
        /// <param name="request">The registration request containing the passenger's name, email, and password.</param>
        /// <returns>A response indicating the registration status.</returns>
        [HttpPost("Register/Passenger")]
        public IActionResult RegisterPassenger([FromBody] RegisterRequest request)
        {
            try
            {
                _sessionManager.RegisterPassanger(request.Name, request.Email, request.Password);
                _sessionManager.Save();
                return Ok(new { message = "Passenger registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Logout Method

        /// <summary>
        /// Logs out the currently logged-in user.
        /// </summary>
        /// <returns>A response indicating the logout status.</returns>
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            try
            {
                _sessionManager.Logout();
                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Authentication Methods

        /// <summary>
        /// Checks if a user is authenticated.
        /// </summary>
        /// <returns>A response indicating whether the user is authenticated or not.</returns>
        [HttpGet("IsAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            try
            {
                bool isAuthenticated = _sessionManager.IsAuthenticated();
                return Ok(new { isAuthenticated });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the details of the currently logged-in user.
        /// </summary>
        /// <returns>A response with the details of the logged-in user or a message indicating no user is logged in.</returns>
        [HttpGet("GetLoggedInUser")]
        public IActionResult GetLoggedInUser()
        {
            try
            {
                var person = _sessionManager.GetLoggedInPerson();
                if (person != null)
                {
                    return Ok(person);
                }
                return Ok(new { message = "No user is currently logged in" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Display Methods

        /// <summary>
        /// Displays all registered staff members.
        /// </summary>
        /// <returns>A list of registered staff members.</returns>
        [HttpGet("DisplayStaff")]
        public IActionResult DisplayStaff()
        {
            try
            {
                var staffList = _sessionManager
                    .GetType()
                    .GetField("_staff", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(_sessionManager) as List<Staff>;
                return Ok(staffList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Displays all registered passengers.
        /// </summary>
        /// <returns>A list of registered passengers.</returns>
        [HttpGet("DisplayPassengers")]
        public IActionResult DisplayPassengers()
        {
            try
            {
                var passengerList = _sessionManager
                    .GetType()
                    .GetField("_passengers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(_sessionManager) as List<Passenger>;
                return Ok(passengerList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Reservation Methods

        /// <summary>
        /// Retrieves all reservations for the logged-in passenger.
        /// </summary>
        /// <returns>A list of reservations or a message indicating no reservations are found.</returns>
        [HttpGet("GetPassengerReservations")]
        public IActionResult GetPassengerReservations()
        {
            try
            {
                var loggedInPassenger = _sessionManager.GetLoggedInPerson() as Passenger;
                if (loggedInPassenger == null)
                {
                    return Unauthorized(new { message = "User is not authenticated" });
                }

                var reservations = loggedInPassenger.Reservations;
                if (reservations != null && reservations.Any())
                {
                    List<PassengerReservation> passengerReservations = new List<PassengerReservation>();
                    foreach (PassengerReservation p in reservations.Values)
                    {
                        passengerReservations.Add(p);
                    }
                    return Ok(passengerReservations);
                }

                return Ok(new { message = "No reservations found for the logged-in passenger" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Adds a reservation for the logged-in passenger.
        /// </summary>
        /// <param name="reservationDetails">The reservation details for the passenger.</param>
        /// <returns>A response indicating the status of the reservation addition.</returns>
        [HttpPost("AddPassengerReservation")]
        public IActionResult AddPassengerReservation([FromBody] ReservationPerson reservationDetails)
        {
            try
            {
                var loggedInPassenger = _sessionManager.GetLoggedInPerson() as Passenger;
                if (loggedInPassenger == null)
                {
                    return Unauthorized(new { message = "User is not authenticated" });
                }

                var reservations = loggedInPassenger.Reservations;
                PassengerReservation newPassengerReservation = new PassengerReservation()
                {
                    FlightNumber = reservationDetails.FlightNumber,
                    ReservationCode = reservationDetails.ReservationCode,
                };

                loggedInPassenger.AddReservation(newPassengerReservation);

                return Ok(new { message = "Passenger reservation added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}
