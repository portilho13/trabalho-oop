using System.Text.Json;

namespace trabalho_oop;

public class Fleet
{
    private Dictionary<string, Airplane> fleet;
    
    public FMS Fms { get; set; }

    public Fleet(FMS fms)
    {
        Fms = fms;
        fleet = new Dictionary<string, Airplane>();
    }

    public void AddAirplane(Airplane airplane)
    {
        fleet.Add(airplane.Registration, airplane);
    }

    public void ReadAircraftListFromAirplaneFolder()
    {
        Console.WriteLine(Fms.AircraftFolderPath);
        string[] files = Directory.GetFiles(Fms.AircraftFolderPath);
        foreach (string file in files)
        {
            string json = Fms.ReadFromJson(file);
            Console.WriteLine(json);
            Airplane airplane = JsonSerializer.Deserialize<Airplane>(json);
            this.AddAirplane(airplane);
        }
    }

    public void ShowAircraftList()
    {
        foreach (Airplane airplane in fleet.Values)
        {
            Console.WriteLine(airplane);
        }
    }

    public Airplane GetAirplane(string registration)
    {
        return fleet[registration];
    }
}