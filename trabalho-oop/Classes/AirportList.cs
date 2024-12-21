
using System.Text.Json;

namespace trabalho_oop;

public class AirportList
{
    private Dictionary<string, Airport> _airports = new Dictionary<string, Airport>();
    
    private readonly ILogger _logger;

    public AirportList(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
    }
    
    private bool DoesPlaneExist(string registration) => _airports.ContainsKey(registration);
    
    public void AddAirport(Airport airport)
    {
        if (DoesPlaneExist(airport.ICAO))
        {
            throw new InvalidOperationException($"An airplane with registration {airport.ICAO} already exists.");
        }
            
        _airports.Add(airport.ICAO, airport);
        _logger.Info($"Airplane {airport.ICAO} added to fleet.");
    }
    
    public Airport? GetAirport(string icao)
    {
        if (_airports.TryGetValue(icao, out var airport))
        {
            return airport;
        }

        return null; // Return null if the registration does not exist
    }
    
    public void LoadAirports()
    {
        // Retrieves a list of files containing airplane data
        string[] files = FMS.Instance.ReadAirportsFromFolder();

        foreach (string file in files)
        {
            // Reads the JSON data from the file
            string json = FMS.Instance.ReadFromJson(file);

            // Deserializes the JSON data into an Airplane object
            Airport airport = JsonSerializer.Deserialize<Airport>(json);

            // Adds the deserialized airplane to the fleet
            this.AddAirport(airport);
        }
        
    }
    
    public List<String> GetAirportsICAO()
    {
        List<String> airportList = new List<string>();
        foreach (Airport airport in _airports.Values)
        {
            airportList.Add(airport.ICAO);
        }
        return airportList;
    }
    
    public void RemoveAirport(string icao)
    {
        // Retrieves the airplane to be removed
        Airport airport = GetAirport(icao);

        // Deletes the airplane data from the FMS system
        FMS.Instance.DeleteAirport(airport);

        // Removes the airplane from the fleet
        _airports.Remove(icao);

        _logger.Info($"Airport {airport.ICAO} removed from airports.");
    }
}