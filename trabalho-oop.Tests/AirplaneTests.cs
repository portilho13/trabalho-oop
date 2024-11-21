using NUnit.Framework;
using System;

namespace trabalho_oop.Tests
{
    [TestFixture]
    public class AirplaneTests
    {
        private TestLogger _logger;

        // This method runs before each test
        [SetUp]
        public void Setup()
        {
            // Ensure logger is instantiated and ready for tests
            _logger = new TestLogger();
        }

        [Test]
        public void Constructor_ValidParameters_LogsCreation()
        {
            // Arrange
            var company = "Ryanair";
            var registration = "RY12345";
            var capacity = 200;
            var model = "Boeing 737";

            // Act
            var airplane = new Airplane(company, registration, capacity, model, _logger);

            // Assert
            Assert.AreEqual(company, airplane.Company);
            Assert.AreEqual(registration, airplane.Registration);
            Assert.AreEqual(capacity, airplane.Capacity);
            Assert.AreEqual(model, airplane.Model);
            Assert.IsFalse(airplane.IsOccupied);
            Assert.That(_logger.LoggedMessages, Does.Contain($"INFO: Airplane {registration} created successfully. Company: {company}, Capacity: {capacity}"));
        }

        [Test]
        public void Constructor_InvalidParameters_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new Airplane("", "RY12345", 200, "Boeing 737", _logger));

            Assert.That(ex.Message, Does.Contain("Failed to create airplane due to invalid parameters"));
        }

        [Test]
        public void ChangeOccupiedStatus_TogglesStatus_LogsChange()
        {
            // Arrange
            var airplane = new Airplane("Ryanair", "RY12345", 200, "Boeing 737", _logger);

            // Act
            airplane.ChangeOccupiedStatus();

            // Assert
            Assert.IsTrue(airplane.IsOccupied);
            Assert.That(_logger.LoggedMessages, Does.Contain($"INFO: Airplane {airplane.Registration} occupied status changed to True."));
        }

        [Test]
        public void ConvertToJson_ValidAirplane_ReturnsJsonString()
        {
            // Arrange
            var airplane = new Airplane("Ryanair", "RY12345", 200, "Boeing 737", _logger);

            // Act
            var json = airplane.ConvertToJson();

            // Assert
            Assert.That(json, Does.Contain("\"Company\": \"Ryanair\""));
            Assert.That(json, Does.Contain("\"Registration\": \"RY12345\""));
            Assert.That(json, Does.Contain("\"Capacity\": 200"));
        }

        [Test]
        public void GetIdentifier_ValidRegistration_ReturnsRegistration()
        {
            // Arrange
            var airplane = new Airplane("Ryanair", "RY12345", 200, "Boeing 737", _logger);

            // Act
            var identifier = airplane.GetIdentifier();

            // Assert
            Assert.AreEqual("RY12345", identifier);
        }

        [Test]
        public void GetEntityType_ReturnsAirplane()
        {
            // Arrange
            var airplane = new Airplane("Ryanair", "RY12345", 200, "Boeing 737", _logger);

            // Act
            var entityType = airplane.GetEntityType();

            // Assert
            Assert.AreEqual(EntityType.Airplane, entityType);
        }

        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new Airplane("Ryanair", "RY12345", 200, "Boeing 737", null));

            // Assert that the exception's message contains 'logger'
            Assert.That(ex.Message, Does.Contain("logger"));
        }
    }
}
