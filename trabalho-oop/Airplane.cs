using System;
using System.Text.Json;

namespace trabalho_oop
{
    public class Airplane
    {
        public string Company { get; set; }

        public string Registration { get; set; }

        public bool isOccupied { get; set; }
        
        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        public Airplane() { } // Parameterless constructor (needed for deserialization)
        
        public Airplane(string company, string registration, FMS fms)
        {
            Company = company;
            Registration = registration;
            isOccupied = false;
        }
        
        public void ChangeOccupiedStatus()
        {
            isOccupied = !isOccupied;
        }
        ~Airplane() { }
    }
}