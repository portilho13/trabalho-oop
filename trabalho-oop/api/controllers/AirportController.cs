using Microsoft.AspNetCore.Mvc;

namespace trabalho_oop.api.controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportController: ControllerBase
{
    private readonly Airport _airport;

    public AirportController(Airport airport)
    {
        _airport = airport;
    }
    
    

}