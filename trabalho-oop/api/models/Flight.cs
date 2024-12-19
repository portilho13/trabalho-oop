namespace trabalho_oop.api.models;

public struct Airport
{
    public string Name { get; set; }
    public string IATA { get; set; }
    public string ICAO { get; set; }
}

public struct Airplane
{
    public string Company { get; set; }
    public string Registration { get; set; }
    public bool IsOccupied { get; set; }
    public int Capacity { get; set; }
    public string Model { get; set; }
}
public struct Flight
{
    public string Number { get; set; }
    public Airport Origin { get; set; }
    public Airport Destination { get; set; }
    public Airplane Airplane { get; set; }
    public DateTime ScheduledDateTime { get; set; }
}