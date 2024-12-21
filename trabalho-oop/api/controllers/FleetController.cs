//-----------------------------------------------------------------
//    <copyright file="FleetController.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>21-12-2024</date>
//    <time>17:15</time>
//    <version>1.0</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using trabalho_oop;

namespace trabalho_oop.api.controllers
{
    #region Class Documentation

    /// <summary>
    /// The FleetController class provides API endpoints for managing the fleet of airplanes.
    /// It handles actions such as retrieving, creating, and deleting airplanes from the fleet.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FleetController : ControllerBase
    {
        #endregion

        #region Fields

        // The Fleet instance used for retrieving and managing airplane data
        private readonly Fleet _fleet;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the FleetController class.
        /// </summary>
        /// <param name="fleet">The fleet instance used by the controller to access airplane data.</param>
        public FleetController(Fleet fleet)
        {
            _fleet = fleet ?? throw new ArgumentNullException(nameof(fleet), "Fleet is null.");
        }

        #endregion

        #region GET Methods

        /// <summary>
        /// Retrieves a list of all airplanes in the fleet.
        /// </summary>
        /// <returns>A list of airplanes wrapped in a 200 OK response</returns>
        [HttpGet]
        public IActionResult GetFleet()
        {
            var aircraftList = _fleet.GetAirplaneRegistrations();
            List<Airplane> airplaneList = new List<Airplane>();

            // Iterate through the airplane registrations and fetch the corresponding airplane information
            foreach (var aircraft in aircraftList)
            {
                Airplane airplane = _fleet.GetAirplane(aircraft);
                airplaneList.Add(airplane);
            }

            return Ok(airplaneList); // Return the list of airplanes in the response
        }

        /// <summary>
        /// Retrieves information about a specific airplane by its registration.
        /// </summary>
        /// <param name="registration">The registration number of the airplane to fetch</param>
        /// <returns>Airplane information wrapped in a 200 OK response, or a 404 if not found</returns>
        [HttpGet("{registration}")]
        public IActionResult GetAirplaneByRegistration(string registration)
        {
            var airplane = _fleet.GetAirplane(registration);

            if (airplane == null)
            {
                // Return a NotFound status with a message if the airplane is not found
                return NotFound(new { Message = $"Airplane with registration '{registration}' not found." });
            }

            return Ok(airplane); // Return the found airplane in the response
        }

        #endregion

        #region POST Method

        /// <summary>
        /// Creates a new airplane using the provided data.
        /// </summary>
        /// <param name="airplane">The airplane data to create</param>
        /// <returns>201 Created response with the new airplane, or an error message</returns>
        [HttpPost]
        public IActionResult CreateAirplane([FromBody] Airplane airplane)
        {
            if (airplane == null)
            {
                // Return a BadRequest if the airplane data is null or invalid
                return BadRequest(new { Message = "Airplane data is missing or invalid." });
            }

            var existingAirplane = _fleet.GetAirplane(airplane.Registration);
            if (existingAirplane != null)
            {
                // Return a Conflict response if an airplane with the same registration already exists
                return Conflict(new { Message = $"An airplane with registration '{airplane.Registration}' already exists." });
            }

            // Add the new airplane to the fleet
            _fleet.AddAirplane(airplane);
            
            // Save the airplane (ensure FMS.Instance.Save is implemented correctly)
            FMS.Instance.Save(airplane);

            // Return a CreatedAtAction with the newly created airplane and location
            return CreatedAtAction(nameof(GetAirplaneByRegistration), new { registration = airplane.Registration }, airplane);
        }

        #endregion

        #region DELETE Method

        /// <summary>
        /// Deletes an airplane by its registration number.
        /// </summary>
        /// <param name="registration">The registration number of the airplane to delete</param>
        /// <returns>200 OK response with a message confirming deletion</returns>
        [HttpDelete("{registration}")]
        public IActionResult DeleteAirplane(string registration)
        {
            // Remove the airplane from the fleet
            _fleet.RemoveAirplane(registration);

            return Ok(new { Message = "Airplane deleted." }); // Return confirmation message
        }

        #endregion
    }
}
