using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace trabalho_oop
{
    public class Passenger : Person, IStorable
    {
        public string password;
        public string Id { get; set; }
        
        public Dictionary<string, Reservation> Reservations { get; private set; } = new Dictionary<string, Reservation>();

        // Parameterless constructor (needed for deserialization)
        public Passenger()
        {
            Id = NumberGenerator.GenerateRandomNumber();
        }

        public string GetIdentifier() => Id;
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
                throw new JsonException($"Failed to serialize passenger {Name} to JSON", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error while converting passenger {Name} to JSON", ex);
            }
        }
        public EntityType GetEntityType() => EntityType.Passenger;

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
    }
}