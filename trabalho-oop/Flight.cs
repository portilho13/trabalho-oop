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
        public Airplane Airplane { get; private set; }
        
        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        
        public Flight(string number, string origin, string destination, Airplane airplane) {
            airplane.ChangeOccupiedStatus(); // Change status to occupied
            Number = number;
            Origin = origin;
            Destination = destination;
            Airplane = airplane;
        }

        ~Flight() { }

    }
}
