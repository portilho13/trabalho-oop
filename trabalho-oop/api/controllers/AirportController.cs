using Microsoft.AspNetCore.Mvc;

namespace trabalho_oop.api.controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportController : ControllerBase
{
    private readonly AirportList _airports;

    public AirportController(AirportList airportList)
    {
        _airports = airportList;
    }

    // GET api/airport
    [HttpGet]
    public IActionResult GetAirports()
    {
        var aircraftList = _airports.GetAirportsICAO();
        return Ok(aircraftList);
    }

    // GET api/airport/{icao}
    [HttpGet("{icao}")]
    public IActionResult GetAirportByIcao(string icao)
    {
        var airport = _airports.GetAirport(icao);

        if (airport == null)
        {
            // Use the route parameter directly in the error message
            return NotFound(new { Message = $"Airport with icao '{icao}' not found." });
        }

        return Ok(airport);
    }

    // POST api/airport
    [HttpPost]
    public IActionResult CreateAirport([FromBody] Airport airport)
    {
        if (airport == null)
        {
            return BadRequest(new { Message = "Airport data is missing or invalid." });
        }

        var existingAirport = _airports.GetAirport(airport.ICAO);
        if (existingAirport != null)
        {
            return Conflict(new { Message = $"An airport with icao '{airport.ICAO}' already exists." });
        }

        _airports.AddAirport(airport);
        
        // Save the airport (ensure FMS.Instance.Save is implemented correctly)
        FMS.Instance.Save(airport);

        // Correctly map the route parameter
        return CreatedAtAction(nameof(GetAirportByIcao), new { icao = airport.ICAO }, airport);
    }
}