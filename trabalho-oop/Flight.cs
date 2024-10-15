using System;

namespace trabalho_oop
{
    public class Flight
    {
        public string Origin { get; set; }
        public string Destination { get; set; }

        public Airplane Airplane { get; set; }

        public Flight(string origin, string destination, Airplane airplane) {
            airplane.ChangeOccupiedStatus(); // Change status to occupied
            Origin = origin;
            Destination = destination;
            Airplane = airplane;
        }

        ~Flight() { }

    }
}
