using NUnit.Framework;
using System;

namespace trabalho_oop.Tests
{
    [TestFixture]
    public class AirplaneTests
    {
        private TestLogger _logger;
        
        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();
        }

        [Test]
        public void Constructor_ValidParameters_LogsCreation()
        {
            // Arrange
            var company = "Ryanair";
            var registration = "RYR2345";
            var capacity = 200;
            var model = "Boeing 737";

            // Act
            var airplane = new Airplane(company, registration, capacity, model);
            airplane.SetLogger(_logger);

            // Assert
            Assert.That(company, Is.EqualTo(airplane.Company));
            Assert.That(registration, Is.EqualTo(airplane.Registration));
            Assert.That(capacity, Is.EqualTo(airplane.Capacity));
            Assert.That(model, Is.EqualTo(airplane.Model));
            Assert.IsFalse(airplane.IsOccupied);
        }

        [Test]
        public void Constructor_InvalidParameters_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new Airplane("", "RY12345", 200, "Boeing 737"));
        }

        [Test]
        public void ChangeOccupiedStatus_TogglesStatus_LogsChange()
        {
            // Arrange
            var airplane = new Airplane("Ryanair", "RY12345", 200, "Boeing 737");

            // Act
            airplane.ChangeOccupiedStatus();

            // Assert
            Assert.IsTrue(airplane.IsOccupied);
        }

        [Test]
        public void ConvertToJson_ValidAirplane_ReturnsJsonString()
        {
            // Arrange
            var airplane = new Airplane("Ryanair", "RY12345", 200, "Boeing 737");

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
            var airplane = new Airplane("Ryanair", "EI-GSG", 200, "Boeing 737");

            // Act
            var identifier = airplane.GetIdentifier();

            // Assert
            Assert.That(identifier, Is.EqualTo("EI-GSG"));
        }

        [Test]
        public void GetEntityType_ReturnsAirplane()
        {
            // Arrange
            var airplane = new Airplane("Ryanair", "RY12345", 200, "Boeing 737");

            // Act
            var entityType = airplane.GetEntityType();

            // Assert
            Assert.That(EntityType.Airplane, Is.EqualTo(entityType));
        }

        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            Airplane a = new Airplane("Ryanair", "EI-ABC", 200, "Boeing 737");
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                a.SetLogger(null));
            
        }
    }
}