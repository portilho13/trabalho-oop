using System;
using System.Collections.Generic;
using System.Text.Json;

namespace trabalho_oop
{
    public class Flight : IStorable
    {
        public string Number { get; set; }
        public string Origin { get; private set; }
        public string Destination { get; private set; }
        public Airplane Airplane { get; private set; }
        public Dictionary<string, Reservation> PassengersReservations { get; private set; }

        public string GetIdentifier() => Number;

        public string ConvertToJson()
        {
            try
            {
                return JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Failed to serialize flight {Number} to JSON", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error while converting airplane {Number} to JSON", ex);
            }
        }
        public EntityType GetEntityType() => EntityType.Flight;

        private int GenerateRandomNumberOfPassengers()
        {
            Random random = new Random();
            return random.Next(100, Airplane.Capacity); // Random number between 100 (to be realistic) and Airplane maximum capacity
        }

        public void AddReservation(Passenger passenger)
        {
            Reservation reservation;
            do
            {
                reservation = new Reservation(passenger);
            }while(PassengersReservations.ContainsKey(reservation.ReservationCode));
            passenger.AddReservation(reservation);
            PassengersReservations.Add(reservation.ReservationCode, reservation);
        }

        public Flight(string number, string origin, string destination, Airplane airplane)
        {
            ValidateConstructorParameters(number, origin, destination, airplane);
            Logger.Instance().Info($"Creating flight {number} from {origin} to {destination} using airplane {airplane.Registration}");
            
            airplane.ChangeOccupiedStatus(); // Change status to occupied
            Number = number;
            Origin = origin;
            Destination = destination;
            Airplane = airplane;
            PassengerList p = new PassengerList();
            PassengersReservations = p.GeneratePassengerList(GenerateRandomNumberOfPassengers());

            Logger.Instance().Info($"Flight {number} created successfully with {PassengersReservations.Count} passengers.");
        }
        
        private void ValidateConstructorParameters(string number, string origin, string destination, Airplane airplane)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(number))
                    throw new ArgumentException("Flight Number cannot be empty or whitespace.", nameof(number));

                if (string.IsNullOrWhiteSpace(origin))
                    throw new ArgumentException("Origin cannot be empty or whitespace.", nameof(origin));
                
                if (string.IsNullOrWhiteSpace(destination))
                    throw new ArgumentException("Destination cannot be empty or whitespace.", nameof(destination));
                
                if (airplane == null)
                    throw new ArgumentNullException(nameof(airplane), "Airplane cannot be null.");

            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentOutOfRangeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unexpected error during parameter validation", ex);
            }
        }
        
    }
}