using System;
using System.Text.Json;

namespace trabalho_oop
{
    public class Airplane
    {
        public string Company { get; set; }

        public string Registration { get; set; }

        public bool isOccupied { get; set; }

        private FMS Fms;
        public Airplane() { } // Parameterless constructor (needed for deserialization)
        
        public Airplane(string company, string registration, FMS fms)
        {
            Fms = fms;
            Company = company;
            Registration = registration;
            isOccupied = false;
        }

        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        public void ChangeOccupiedStatus()
        {
            isOccupied = !isOccupied;
        }

        public void SaveAircraft()
        {
            string json = this.ConvertToJson();
            string aircraft = this.Registration + ".json";
            string path = Path.Combine(Fms.AircraftFolderPath, aircraft);
            Fms.WriteJsonToFile(path, json);
        }

        ~Airplane() { }
    }
}