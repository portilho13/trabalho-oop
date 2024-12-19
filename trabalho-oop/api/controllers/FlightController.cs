using Microsoft.AspNetCore.Mvc;

namespace trabalho_oop.api.controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightController: ControllerBase
{
    private readonly Flights _flightList;
    private readonly ILogger _logger;
    public FlightController(Flights flights, ILogger logger)
    {
        _flightList = flights ?? throw new ArgumentNullException(nameof(flights), "flightlist is null.");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "logger is null.");
    }

    [HttpGet]
    public IActionResult GetFlights()
    {
        List<Flight> ListOfFlightss = new List<Flight>();
        var flightList = _flightList.GetFlights();
        foreach (var flight in flightList)
        {
            Flight f = _flightList.GetFlight(flight);
            ListOfFlightss.Add(f);
        }
        return Ok(ListOfFlightss);
    }

    [HttpPost]
    public IActionResult AddFlight([FromBody] models.Flight flight)
    {

        try
        {
            Airport origin = new Airport(flight.Origin.Name, flight.Origin.IATA, flight.Origin.ICAO, _logger);
            Airport destination = new Airport(flight.Destination.Name, flight.Destination.IATA, flight.Destination.ICAO, _logger);

            Airplane ar = new Airplane(flight.Airplane.Company, flight.Airplane.Registration, flight.Airplane.Capacity,
                flight.Airplane.Model, _logger);
            
            Flight f = new Flight(flight.Number, origin, destination, ar, flight.ScheduledDateTime, _logger);
            
            _flightList.AddFlight(f);
            
            FMS.Instance.Save(f);

            return Ok(new { message = $"Flight {flight.Number} added successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while adding the flight.", error = ex.Message });
        }
    }

    [HttpDelete(template: "{flightNumber}")]
    public IActionResult DeleteFlight(string flightNumber)
    {
        _flightList.DeleteFlight(flightNumber);
        return Ok(new { message = $"Flight {flightNumber} deleted successfully." });
    }


    [HttpGet("{flightNumber}")]
    public IActionResult GetFlight(string flightNumber)
    {
        return Ok(_flightList.GetFlight(flightNumber));
    }
}