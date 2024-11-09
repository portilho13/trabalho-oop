using System;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;

namespace trabalho_oop
{
    public class Flight
    {
        private FMS Fms;
        public string Number { get; set; }
        public string Origin { get; private set; }
        public string Destination { get; private set; }
        public Airplane Airplane { get; private set; }
        
        public Dictionary<string, Reservation> PassangersReservations { get; private set; }
        
        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        private int GenerateRandomNumberOfPassangers()
        {
            Random random = new Random();
            return random.Next(100, Airplane.Capacity); // Random number between 100 (to be realistic) and Airplane maximum capacity
        }

        public void DisplayPassengers()
        {
            if (PassangersReservations.Count == 0)
            {
                Console.WriteLine("No passengers to display.");
                return;
            }

            Console.WriteLine("Passenger List:");
            foreach (var entry in PassangersReservations)
            {
                string reservationCode = entry.Key;
                Reservation passenger = entry.Value;

                Console.WriteLine($"Reservation Code: {reservationCode}, Name: {passenger.Passanger.Name}");
            }
        }

        public void AddReservation(Reservation reservation)
        {
            PassangersReservations.Add(reservation.ReservationCode, reservation);
        }
        public Flight(string number, string origin, string destination, Airplane airplane, FMS fms) {
            Fms = fms;
            airplane.ChangeOccupiedStatus(); // Change status to occupied
            Number = number;
            Origin = origin;
            Destination = destination;
            Airplane = airplane;
            PassangerList p = new PassangerList(Fms);
            PassangersReservations = p.GeneratePassangerList(GenerateRandomNumberOfPassangers());
        }

        ~Flight() { }

    }
}
