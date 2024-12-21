using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace trabalho_oop.Tests
{
    [TestFixture]
    public class FleetTests
    {
        private TestLogger _logger;
        private Fleet _fleet;
        private Airplane _testAirplane;
        private FMS _fms;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Initialize the logger first since FMS depends on it
            _logger = new TestLogger();

            // Initialize FMS with the logger
            FMS.InitializeLogger(_logger);
            var fms = FMS.Instance;
            fms.Start(_logger);
        }

        [SetUp]
        public void Setup()
        {
            // Reset logger for each test
            _logger = new TestLogger();

            // Create new fleet instance
            _fleet = new Fleet(_logger);

            // Create test airplane
            var company = "Ryanair";
            var registration = "EI-ABC";
            var capacity = 200;
            var model = "Boeing 737";

            _testAirplane = new Airplane(company, registration, capacity, model, _logger);
        }

        [Test]
        public void AddAirplane_UniqueRegistration_AddsSuccessfully()
        {
            // Act
            _fleet.AddAirplane(_testAirplane);

            // Assert
            Assert.That(_testAirplane, Is.EqualTo(_fleet.GetAirplane("EI-ABC")));
            Assert.That(_logger.LoggedMessages, Does.Contain("INFO: Airplane EI-ABC added to fleet."));
        }

        [Test]
        public void AddAirplane_DuplicateRegistration_ThrowsInvalidOperationException()
        {
            // Arrange
            _fleet.AddAirplane(_testAirplane);

            // Act
            var duplicateAirplane = new Airplane(
                _testAirplane.Company,
                _testAirplane.Registration,
                _testAirplane.Capacity,
                _testAirplane.Model,
                _logger
            );

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _fleet.AddAirplane(duplicateAirplane)
            );
            Assert.That(exception.Message,
                Is.EqualTo($"An airplane with registration {_testAirplane.Registration} already exists."));
        }

        [Test]
        public void GetAirplane_ExistingRegistration_ReturnsAirplane()
        {
            // Arrange
            _fleet.AddAirplane(_testAirplane);

            // Act
            var retrievedAirplane = _fleet.GetAirplane("EI-ABC");

            // Assert
            Assert.That(_testAirplane, Is.EqualTo(retrievedAirplane));
        }

        [Test]
        public void RemoveAirplane_ExistingAirplane_RemovesSuccessfully()
        {
            // Arrange
            _fleet.AddAirplane(_testAirplane);

            // Act
            _fleet.RemoveAirplane("EI-ABC");

            // Assert
            Assert.That(_fleet.GetAirplane("EI-ABC"), Is.Null);
            Assert.That(_logger.LoggedMessages, Does.Contain("INFO: Airplane EI-ABC removed from fleet."));
        }

        [Test]
        public void RemoveAirplane_NonExistingAirplane_ThrowsKeyNotFoundException()
        {
            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _fleet.RemoveAirplane("NON-EXISTING")
            );
            Assert.That(exception.Message, Is.EqualTo("The airplane with registration NON-EXISTING does not exist."));
        }

        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new Fleet(null));
            Assert.That(exception.ParamName, Is.EqualTo("logger"));
        }

        [Test]
        public void AddAirplane_NullAirplane_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _fleet.AddAirplane(null));
            Assert.That(exception.ParamName, Is.EqualTo("airplane"));
        }
    }
}