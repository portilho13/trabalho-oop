using Microsoft.AspNetCore.Mvc;
using trabalho_oop;

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
        var aircraftList = _fleet.GetAircraftList();
        return Ok(aircraftList);
    }
    
}