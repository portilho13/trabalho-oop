using System.Text.Json;

namespace trabalho_oop;

public class Fleet
{
    private Dictionary<string, Airplane> _fleet = new Dictionary<string, Airplane>();

    private bool DoesPlaneExist(string Resistration) => _fleet.ContainsKey(Resistration);

    public Airplane GetAirplane(string Resistration)
    {
        return _fleet[Resistration];
    }

    public void AddAirplane(Airplane airplane)
    {
        _fleet.Add(airplane.Registration, airplane);
    }

    public void LoadFleet()
    {
        string[] files = FMS.Instance.ReadAirplaneFromFolder();
        foreach (string file in files)
        {
            string json = FMS.Instance.ReadFromJson(file);
            Airplane airplane = JsonSerializer.Deserialize<Airplane>(json);
            this.AddAirplane(airplane);
        }
    }

    public void ShowAircraftList()
    {
        foreach (Airplane airplane in _fleet.Values)
        {
            Console.WriteLine(airplane.Registration);
        }
    }
    

    public void RemoveAirplane(string registration)
    {
        Airplane airplane = GetAirplane(registration);
        FMS.Instance.DeleteAirplane(airplane);
        _fleet.Remove(registration);
    }
    
}