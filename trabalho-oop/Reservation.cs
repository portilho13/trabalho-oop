using System;
using System.Text.Json;

namespace trabalho_oop
{
    public class Reservation
    {
        private static Logger logger = Logger.Instance("./fms/logs/app.log");

        [NonSerialized] public string ReservationCode;
        public Person Passenger { get; set; }

        public Reservation(Passenger passenger)
        {
            Passenger = passenger;
            GenerateReservationCode();
        }
        
        public Reservation() {}
        
        public string GetIdentifier() => ReservationCode;
        
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
    }
}