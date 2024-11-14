using System;
using System.Text.Json;

namespace trabalho_oop
{
    [Serializable]
    public class Reservation : IStorable
    {
        private static Logger logger = Logger.Instance("./fms/logs/app.log");

        public FMS Fms { private get; set; }

        [NonSerialized] public string ReservationCode;
        public Person Passenger { get; set; }

        public Reservation(Passenger passenger)
        {
            Passenger = passenger;
            GenerateReservationCode();
        }
        
        public Reservation() {}

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
                throw new JsonException($"Failed to serialize passenger {ReservationCode} to JSON", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error while converting passenger {ReservationCode} to JSON", ex);
            }
        }
        public string GetIdentifier() => ReservationCode;

        public EntityType GetEntityType() => EntityType.Reservation;

        // Generate a unique reservation code
        public void GenerateReservationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var reservationCode = new char[6];

            for (int i = 0; i < reservationCode.Length; i++)
            {
                reservationCode[i] = chars[random.Next(chars.Length)];
            }

            this.ReservationCode = new string(reservationCode);
            
        }
        

        ~Reservation()
        {
        }
    }
}