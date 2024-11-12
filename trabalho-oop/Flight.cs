using System;
using System.Collections.Generic;
using System.Text.Json;

namespace trabalho_oop
{
    public class Flight : IStorable
    {
        private FMS Fms;
        public string Number { get; set; }
        public string Origin { get; private set; }
        public string Destination { get; private set; }
        public Airplane Airplane { get; private set; }
        public Dictionary<string, Reservation> PassengersReservations { get; private set; }

        public string GetIdentifier() => Number;

        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        public EntityType GetEntityType() => EntityType.Flight;

        private int GenerateRandomNumberOfPassengers()
        {
            Random random = new Random();
            return random.Next(100, Airplane.Capacity); // Random number between 100 (to be realistic) and Airplane maximum capacity
        }

        public void AddReservation(Reservation reservation)
        {
            if (!PassengersReservations.ContainsKey(reservation.ReservationCode))
            {
                PassengersReservations.Add(reservation.ReservationCode, reservation);
                Logger.Instance().Info($"Reservation with code {reservation.ReservationCode} added.");
            }
            else
            {
                Logger.Instance().Error($"Reservation with code {reservation.ReservationCode} already exists.");
            }
        }

        public Flight(string number, string origin, string destination, Airplane airplane, FMS fms)
        {
            Logger.Instance().Info($"Creating flight {number} from {origin} to {destination} using airplane {airplane.Registration}");
            
            Fms = fms;
            airplane.ChangeOccupiedStatus(); // Change status to occupied
            Number = number;
            Origin = origin;
            Destination = destination;
            Airplane = airplane;
            PassengerList p = new PassengerList(Fms);
            PassengersReservations = p.GeneratePassengerList(GenerateRandomNumberOfPassengers());

            Logger.Instance().Info($"Flight {number} created successfully with {PassengersReservations.Count} passengers.");
        }

        ~Flight() { }
    }
}
