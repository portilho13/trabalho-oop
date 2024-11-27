using NUnit.Framework;
using System;
using trabalho_oop;

namespace trabalho_oop.Tests
{
    [TestFixture]
    public class AirportTests
    {
        private TestLogger _logger;
        private Airport _airport;

        [SetUp]
        public void Setup()
        {
            _logger = new TestLogger();
            _airport = new Airport("Lisbon Airport", "LIS", "LPPT");
        }

        [Test]
        public void Constructor_ValidParameters_CreatesAirportInstance()
        {
            Assert.That(_airport.AirportName, Is.EqualTo("Lisbon Airport"));
            Assert.That(_airport.IATA, Is.EqualTo("LIS"));
            Assert.That(_airport.ICAO, Is.EqualTo("LPPT"));
        }

        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            Airport airport = new Airport("Lisbon Airport", "LIS", "LPPT");
            Assert.Throws<ArgumentNullException>(() => airport.SetLogger(null));
        }

        [Test]
        public void Constructor_EmptyAirportName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Airport("", "LIS", "LPPT"));
        }

        [Test]
        public void Constructor_EmptyIATA_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Airport("Lisbon Airport", "", "LPPT"));
        }

        [Test]
        public void Constructor_EmptyICAO_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Airport("Lisbon Airport", "LIS", ""));
        }

        [Test]
        public void ConvertToJson_SerializesAirportCorrectly()
        {
            string json = _airport.ConvertToJson();
            Assert.That(json, Does.Contain(_airport.AirportName));
            Assert.That(json, Does.Contain(_airport.IATA));
            Assert.That(json, Does.Contain(_airport.ICAO));
        }

        [Test]
        public void GetIdentifier_ReturnsICAO()
        {
            Assert.That(_airport.GetIdentifier(), Is.EqualTo(_airport.ICAO));
        }

        [Test]
        public void GetEntityType_ReturnsAirportType()
        {
            Assert.That(_airport.GetEntityType(), Is.EqualTo(EntityType.Airport));
        }
    }
}