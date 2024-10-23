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

    public void LoadFleet()
    {
        string[] files = Fms.ReadAirplaneFromFolder();
        foreach (string file in files)
        {
            string json = Fms.ReadFromJson(file);
            Airplane airplane = JsonSerializer.Deserialize<Airplane>(json);
            this.AddAirplane(airplane);
        }
    }

    public void ShowAircraftList()
    {
        foreach (Airplane airplane in fleet.Values)
        {
            Console.WriteLine(airplane.Registration);
        }
    }

    public Airplane GetAirplane(string registration)
    {
        return fleet[registration];
    }

    public void RemoveAirplane(string registration)
    {
        Airplane airplane = GetAirplane(registration);
        Fms.DeleteAirplane(airplane);
        fleet.Remove(registration);
    }
    
}