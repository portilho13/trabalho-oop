using System;
using System.Text.Json;

namespace trabalho_oop
{
    public class Reservation : IStorable
    {
        private static Logger logger = Logger.Instance("./fms/logs/app.log");

        public FMS Fms { private get; set; }

        public string ReservationCode { get; private set; }
        public Person Passenger { get; set; }

        public Reservation()
        {
        }

        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

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