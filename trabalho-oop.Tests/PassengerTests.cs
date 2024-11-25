using NUnit.Framework;
using System;
using System.Collections.Generic;
using trabalho_oop;

namespace trabalho_oop.Tests
{
    [TestFixture]
    public class PassengerTests
    {
        private TestLogger _testLogger;
        private Passenger _passenger;

        [SetUp]
        public void Setup()
        {
            _testLogger = new TestLogger();
            _passenger = new Passenger(_testLogger);
        }

        [Test]
        public void Constructor_GeneratesUniqueId()
        {
            string firstPassengerId = _passenger.Id;
            Passenger secondPassenger = new Passenger(_testLogger);
            Assert.That(secondPassenger.Id, Is.Not.EqualTo(firstPassengerId));
        }

        [Test]
        public void GetIdentifier_ReturnsPassengerId()
        {
            Assert.That(_passenger.GetIdentifier(), Is.EqualTo(_passenger.Id));
        }

        [Test]
        public void ConvertToJson_SerializesPassengerCorrectly()
        {
            string json = _passenger.ConvertToJson();
            Assert.That(json, Does.Contain(_passenger.Id)); 
        }

        [Test]
        public void GetEntityType_ReturnsPassengerType()
        {
            Assert.That(_passenger.GetEntityType(), Is.EqualTo(EntityType.Passenger));
        }

        [Test]
        public void AddReservation_AddsReservationToCollection()
        {
            Reservation reservation = new Reservation(_passenger, _testLogger);
            _passenger.AddReservation(reservation);
            Assert.That(_passenger.Reservations, Contains.Key(reservation.ReservationCode));
            Assert.That(_testLogger.LoggedMessages, Contains.Item($"INFO: Added reservation {reservation.ReservationCode} for passenger " + _passenger.Id + "."));
        }

        [Test]
        public void AddReservation_SkipsExistingReservation()
        {
            Reservation reservation = new Reservation(_passenger, _testLogger);
            _passenger.AddReservation(reservation);
            _passenger.AddReservation(reservation);
            Assert.That(_passenger.Reservations.Count, Is.EqualTo(1));
            Assert.That(_testLogger.LoggedMessages, Contains.Item($"WARN: Reservation {reservation.ReservationCode} already exists for passenger " + _passenger.Id + ". Skipping addition."));
        }
    }
}