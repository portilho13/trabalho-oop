using System;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;

namespace trabalho_oop
{
    public class Flight
    {
        public string Number { get; set; }
        public string Origin { get; private set; }
        public string Destination { get; private set; }
        
        private FMS Fms { get; set; }
        public Airplane Airplane { get; private set; }

        public Flight(string number, string origin, string destination, Airplane airplane, FMS fms) {
            airplane.ChangeOccupiedStatus(); // Change status to occupied
            Fms = fms;
            Number = number;
            Origin = origin;
            Destination = destination;
            Airplane = airplane;
        }

        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        public void SaveFlight()
        {
            string json = this.ConvertToJson();

            string flighFile = this.Number + ".json";

            string path = Path.Combine(Fms.FlightFolderPath, flighFile);
            Console.WriteLine(path);
            Fms.WriteJsonToFile(path, json);
        }

        ~Flight() { }

    }
}
