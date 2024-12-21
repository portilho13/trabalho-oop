using Newtonsoft.Json;

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
            Airport Porto = new Airport("Francisco Sa Carneiro", "OPO", "LPPR", _logger);
            Airport Milan = new Airport("Milan Malpensa", "MXP", "LIMC", _logger);
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
                Porto,
                Milan,
                airplane: _airplane,
                logger: _logger,
                scheduledDateTime: _flightDateTime
            );
        }
        [Test]
        public void Constructor_InvalidParameters_ThrowsArgumentException()
        {
            Airport Porto = new Airport("Francisco Sa Carneiro", "OPO", "LPPR", _logger);
            Airport Milan = new Airport("Milan Malpensa", "MXP", "LIMC", _logger);
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new Flight(
                    number: "", // Invalid empty number
                    Porto,
                    Milan,
                    airplane: _airplane,
                    logger: _logger,
                    scheduledDateTime: _flightDateTime
                ));
        }
        [Test]
        public void Correctly_Adds_ExistingReservation()
        {
            // Arrange
            Passenger passenger = new Passenger()
            {
                Name = "Port",
                Email = "port@gmail.com",
            };
            string reservationCode = _flight.AddReservation("Port");
            // Act
            PassengerReservation p = new PassengerReservation()
            {
                FlightNumber = _flight.Number,
                ReservationCode = reservationCode,
            };    
            passenger.AddReservation(p);
            var firstReservation = passenger.Reservations.First(); // Get the first reservation
            bool reservationExists = _flight.PassengersReservations.ContainsKey(firstReservation.Key);
            // Assert
            Assert.IsTrue(reservationExists, "The reservation was not added to the flight.");

        }
        
        [Test]
        public void Throws_Exception_For_NonExisting_Reservation()
        {
            // Arrange
            string nonExistingReservationCode = "INVALID_CODE";
            // Act & Assert
            var ex = Assert.Throws<KeyNotFoundException>(() => _flight.GetReservation(nonExistingReservationCode));
        }
        
        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            
            Airport Porto = new Airport("Francisco Sa Carneiro", "OPO", "LPPR", _logger);
            Airport Milan = new Airport("Milan Malpensa", "MXP", "LIMC", _logger);
    
            Assert.Throws<ArgumentNullException>(() => new Flight(
                number: "RZR4703", // Invalid empty number
                Porto,
                Milan,
                airplane: _airplane,
                logger: null,
                scheduledDateTime: _flightDateTime
            ));
        }

        [Test]
        public void JSONConvert_ValidFlight()
        {
            string json = _airplane.ConvertToJson();
            Airplane testAirplane = JsonConvert.DeserializeObject<Airplane>(json);
            Assert.That(testAirplane, Is.Not.Null);
        }
    }
}