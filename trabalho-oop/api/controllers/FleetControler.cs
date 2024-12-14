using Microsoft.AspNetCore.Mvc;
using trabalho_oop;

namespace trabalho_oop.api.controllers;

[ApiController]
[Route("api/[controller]")]
public class FleetController : ControllerBase
{
    private readonly Fleet _fleet;

    public FleetController(Fleet fleet)
    {
        _fleet = fleet;
    }

    // GET api/fleet
    [HttpGet]
    public IActionResult GetFleet()
    {
        var aircraftList = _fleet.GetAirplaneRegistrations();
        return Ok(aircraftList);
    }
    
    [HttpGet("{registration}")]
    public IActionResult GetAirplaneByRegistration(string registration)
    {
        var airplane = _fleet.GetAirplane(registration);

        if (airplane == null)
        {
            // Explicitly return a JSON object with the error message
            return NotFound(new { Message = $"Airplane with registration '{registration}' not found." });
        }

        return Ok(airplane);
    }
    
    // POST api/fleet
    [HttpPost]
    public IActionResult CreateAirplane([FromBody] Airplane airplane)
    {
        if (airplane == null)
        {
            return BadRequest(new { Message = "Airplane data is missing or invalid." });
        }

        var existingAirplane = _fleet.GetAirplane(airplane.Registration);
        if (existingAirplane != null)
        {
            return Conflict(new { Message = $"An airplane with registration '{airplane.Registration}' already exists." });
        }

        _fleet.AddAirplane(airplane);
        
        FMS.Instance.Save(airplane);

        return CreatedAtAction(nameof(GetAirplaneByRegistration), new { registration = airplane.Registration }, airplane);
    }

    
}