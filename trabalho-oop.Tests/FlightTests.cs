namespace trabalho_oop.Tests
{
    [TestFixture]
    public class FlightTests
    {
        private TestLogger _logger;
        private Flight _flight;
        private Airplane _airplane;
        private DateTime _flightDateTime;

        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();
            _flightDateTime = new DateTime(2024, 12, 25, 15, 30, 0);

            // Create the airplane first
            _airplane = new Airplane(
                company: "Ryanair",
                registration: "EI-ABC",
                capacity: 200,
                model: "Boeing 737",
                logger: _logger
            );

            // Then create the flight
            _flight = new Flight(
                number: "RYR4703",
                origin: "LPPR",
                destination: "LPPT",
                airplane: _airplane,
                logger: _logger,
                scheduledDateTime: _flightDateTime
            );
        }

        [Test]
        public void Constructor_InvalidParameters_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new Flight(
                    number: "", // Invalid empty number
                    origin: "LPPR",
                    destination: "LPPT",
                    airplane: _airplane,
                    logger: _logger,
                    scheduledDateTime: _flightDateTime
                ));
        }

        [Test]
        public void Correctly_Adds_ExistingReservation()
        {
            // Arrange
            Passenger passenger = new Passenger(_logger)
            {
                Name = "Port",
                Email = "port@gmail.com",
            };
            _flight.AddReservation(passenger);

            // Act
            var firstReservation = passenger.Reservations.First(); // Get the first reservation
            bool reservationExists = _flight.PassengersReservations.ContainsKey(firstReservation.Key);

            // Assert
            Assert.IsTrue(reservationExists, "The reservation was not added to the flight.");
            Assert.AreEqual(firstReservation.Value, _flight.GetReservation(firstReservation.Key), 
                "The retrieved reservation does not match the added reservation.");
        }
        
        [Test]
        public void Throws_Exception_For_NonExisting_Reservation()
        {
            // Arrange
            string nonExistingReservationCode = "INVALID_CODE";

            // Act & Assert
            var ex = Assert.Throws<KeyNotFoundException>(() => _flight.GetReservation(nonExistingReservationCode));
        }

    }
}