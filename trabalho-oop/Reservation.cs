using System;
using System.Text.Json;

namespace trabalho_oop
{
    public class Reservation
    {
        private readonly Logger _logger;

        [NonSerialized] public string ReservationCode;
        public Person Passenger { get; set; }

        public Reservation(Passenger passenger, Logger logger)
        {
            ValidateConstructorParameters(passenger);
            Passenger = passenger;
            ReservationCode = NumberGenerator.GenerateRandomNumber();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
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
        
        public string GetIdentifier() => ReservationCode;
        
    }
}