using System.Text.Json;

namespace trabalho_oop;

public class Airport: IStorable
{
    
    private readonly ILogger _logger;
    public string AirportName { get; set; }
    public string IATA { get; set; }
    public string ICAO { get; set; }

    public Airport(string airportName, string iata, string icao, ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        ValidateConstructorParameters(airportName, iata, icao);
        AirportName = airportName;
        IATA = iata;
        ICAO = icao;

    }
    
    private void ValidateConstructorParameters(string airportName, string iata, string icao)
    {
        if (string.IsNullOrEmpty(airportName))
        {
            throw new ArgumentNullException(nameof(airportName), "Airport Name cannot be null or empty.");
        }
        
        if (string.IsNullOrEmpty(iata))
        {
            throw new ArgumentNullException(nameof(iata), "IATA cannot be null or empty.");
        }
        
        if (string.IsNullOrEmpty(icao))
        {
            throw new ArgumentNullException(nameof(icao), "ICAO cannot be null or empty.");
        }
    }
    
    public EntityType GetEntityType() => EntityType.Airport;
    
    public string ConvertToJson()
    {
        try
        {
            // Serialize the object to a formatted JSON string
            return JsonSerializer.Serialize(this, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
        }
        catch (JsonException ex)
        {
            // Log the JSON serialization error and throw a detailed exception
            throw new JsonException($"Failed to serialize airport {AirportName} to JSON", ex);
        }
        catch (Exception ex)
        {
            // Catch any other unexpected errors during serialization
            throw new InvalidOperationException($"Unexpected error while converting airport {AirportName} to JSON", ex);
        }
    }

    public string GetIdentifier() => ICAO;

}