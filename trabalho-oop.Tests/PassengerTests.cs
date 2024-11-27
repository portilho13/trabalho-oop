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
            _passenger = new Passenger
            {
                Id = NumberGenerator.GenerateRandomNumber()
            };
        }

        [Test]
        public void Constructor_GeneratesUniqueId()
        {
            string firstPassengerId = _passenger.Id;
            Passenger secondPassenger = new Passenger
            {
                Id = NumberGenerator.GenerateRandomNumber()
            };
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
            PassengerReservation reservation = new PassengerReservation
            {
                FlightNumber = "RYR4703",
                ReservationCode = NumberGenerator.GenerateRandomNumber()
            };
            _passenger.AddReservation(reservation);
            Assert.That(_passenger.Reservations, Contains.Key(reservation.ReservationCode));
        }

        [Test]
        public void AddReservation_SkipsExistingReservation()
        {
            PassengerReservation reservation = new PassengerReservation
            {
                FlightNumber = "RYR4703",
                ReservationCode = NumberGenerator.GenerateRandomNumber()
            };
            _passenger.AddReservation(reservation);
            _passenger.AddReservation(reservation);
            Assert.That(_passenger.Reservations.Count, Is.EqualTo(1));
        }
    }
}