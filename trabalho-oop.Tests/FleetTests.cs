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

        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();
            _fleet = new Fleet(_logger);
            var company = "Ryanair";
            var registration = "EI-ABC";
            var capacity = 200;
            var model = "Boeing 737";

            // Act
            _testAirplane = new Airplane(company, registration, capacity, model, _logger);
        }

        [Test]
        public void AddAirplane_UniqueRegistration_AddsSuccessfully()
        {
            // Act
            _fleet.AddAirplane(_testAirplane);

            // Assert
            Assert.AreEqual(_testAirplane, _fleet.GetAirplane("EI-ABC"));
            Assert.That(_logger.LoggedMessages, 
                Does.Contain("INFO: Airplane EI-ABC added to fleet."));
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
        public void GetAirplane_NonExistingRegistration_ThrowsKeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => 
                _fleet.GetAirplane("NON-EXISTING"));
        }

        [Test]
        public void RemoveAirplane_ExistingAirplane_RemovesSuccessfully()
        {
            // Arrange
            _fleet.AddAirplane(_testAirplane);

            // Act
            _fleet.RemoveAirplane("EI-ABC");

            // Assert
            Assert.Throws<KeyNotFoundException>(() => 
                _fleet.GetAirplane("EI-ABC"));
            Assert.That(_logger.LoggedMessages, 
                Does.Contain("INFO: Airplane EI-ABC removed from fleet."));
        }

        [Test]
        public void RemoveAirplane_NonExistingAirplane_ThrowsKeyNotFoundException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => 
                _fleet.RemoveAirplane("NON-EXISTING"));
        }
        
        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Fleet(null));
        }
    }
}