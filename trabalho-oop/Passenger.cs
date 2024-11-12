using System;
using System.Collections.Generic;
using System.Text.Json;

namespace trabalho_oop
{
    public class Passenger : Person, IStorable
    {
        public string password;
        public string Id { get; set; } = GenerateRandomId();
        public Dictionary<string, Reservation> Reservations { get; set; } = new Dictionary<string, Reservation>();

        // Parameterless constructor (needed for deserialization)
        public Passenger() 
        {
        }

        public string GetIdentifier() => Id;
        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        public EntityType GetEntityType() => EntityType.Passenger;

        private static string GenerateRandomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomId = new char[6];

            for (int i = 0; i < randomId.Length; i++)
            {
                randomId[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomId);
        }

        private bool DoesReservationExists(string reservationCode) => Reservations.ContainsKey(reservationCode);

        public void AddReservation(Reservation reservation)
        {
            if (!DoesReservationExists(reservation.ReservationCode))
            {
                Reservations.Add(reservation.ReservationCode, reservation);
                Logger.Instance().Info($"Added reservation {reservation.ReservationCode} for passenger {Id}.");
            }
            else
            {
                Logger.Instance().Warn($"Reservation {reservation.ReservationCode} already exists for passenger {Id}. Skipping addition.");
            }
        }

        ~Passenger()
        {
            Logger.Instance().Info($"Passenger {Id} instance is being destroyed.");
        }
    }
}
