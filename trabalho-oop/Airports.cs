using System.Text.Json;

namespace trabalho_oop;

public class Airports
{
    private Dictionary<string, Airport> _airports = new Dictionary<string, Airport>();

    private readonly ILogger _logger;

    public Airports(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
    }
    
    private bool DoesAirportExist(string airport) => _airports.ContainsKey(airport);
    
    public Airport GetAirport(string airport) => _airports[airport];

    public void AddAirport(Airport airport)
    {
        if (DoesAirportExist(airport.AirportName))
        {
            throw new InvalidOperationException($"Airport {airport.AirportName} already exists");
        }
        
        _airports.Add(airport.ICAO, airport);
    }
    
    /// <summary>
    /// Loads aiports from the files stored in the FMS system.
    /// Deserializes the JSON files into Airport objects and adds them to the airports list.
    /// </summary>
    public void LoadAirports()
    {
        // Retrieves a list of files containing airport data
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
    
    /// <summary>
    /// Displays the registration numbers of all airplanes in the fleet.
    /// </summary>
    public void ShowAircraftList()
    {   
        foreach (Airport airport in _airports.Values)
        {
            Console.WriteLine(airport.ICAO);
        }
    }
    
    /// <summary>
    /// Removes an Airport from the airports list by its ICAO code.
    /// Also deletes the airport data from the FMS system.
    /// </summary>
    /// <param name="registration">The registration of the airplane to remove.</param>
    public void RemoveAirplane(string registration)
    {
        // Retrieves the airplane to be removed
        Airport airport = GetAirport(registration);

        // Deletes the airplane data from the FMS system
        FMS.Instance.DeleteAirport(airport);

        // Removes the airplane from the fleet
        _airports.Remove(airport.ICAO);

        _logger.Info($"Airport {airport.ICAO} removed from Airports.");
    }
}