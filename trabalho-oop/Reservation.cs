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
            ValidateConstructorParameters(passenger);
            Passenger = passenger;
            ReservationCode = NumberGenerator.GenerateRandomNumber();
        }
        
        private void ValidateConstructorParameters(Passenger passenger)
        {
            try
            {
                if (passenger == null)
                {
                    throw new ArgumentNullException(nameof(passenger), "Passenger cannot be null.");
                }

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unexpected error during parameter validation", ex);
            }
        }

        public Reservation()
        {
            ReservationCode = NumberGenerator.GenerateRandomNumber();
        }
        
        public string GetIdentifier() => ReservationCode;
        
    }
}