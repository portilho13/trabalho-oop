using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace trabalho_oop.api.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionManagerController : ControllerBase
    {
        private readonly SessionManager _sessionManager;

        public SessionManagerController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// Login a staff member.
        /// </summary>
        [HttpPost("Login/Staff")]
        public IActionResult LoginStaff([FromBody] LoginRequest request)
        {
            try
            {   
                Console.WriteLine(request.Email);
                Console.WriteLine(request.Password);
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
        /// Login a passenger.
        /// </summary>
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

        /// <summary>
        /// Register a staff member.
        /// </summary>
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
        /// Register a passenger.
        /// </summary>
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

        /// <summary>
        /// Logout the current user.
        /// </summary>
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

        /// <summary>
        /// Check if a user is authenticated.
        /// </summary>
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
        /// Get details of the logged-in user.
        /// </summary>
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
                return NotFound(new { message = "No user is currently logged in" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Display all registered staff members.
        /// </summary>
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
        /// Display all registered passengers.
        /// </summary>
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
    }

    public class LoginRequest
    {
        public string Email { get; set; } // StaffCode or Email
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}