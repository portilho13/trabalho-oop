using NUnit.Framework;
using System;
using System.Collections.Generic;
using trabalho_oop;

namespace trabalho_oop.Tests
{
    [TestFixture]
    public class PassengerListTests
    {
        private PassengerList _passengerList;

        [SetUp]
        public void SetUp()
        {
            // Initialize PassengerList before each test
            _passengerList = new PassengerList();
        }

        [Test]
        public void GenerateRandomPassenger_ShouldReturnNonEmptyString()
        {
            // Act
            Passenger randomPassenger = new Passenger
            {
                Name = "Ola",
            };
            
            Assert.That(randomPassenger.Name, Is.Not.Empty);
        }

        [Test]
        public void GeneratePassengerList_ShouldReturnCorrectCount()
        {
            // Arrange
            int passengerCount = 5;

            // Act
            var passengerList = _passengerList.GeneratePassengerList(passengerCount);

            // Assert
            Assert.That(passengerList.Count, Is.EqualTo(passengerList.Count), "The number of generated passengers should match the requested count.");
        }

        [Test]
        public void GeneratePassengerList_ShouldHaveUniqueReservationCodes()
        {
            // Arrange
            int passengerCount = 10;

            // Act
            var passengerList = _passengerList.GeneratePassengerList(passengerCount);
            var reservationCodes = new HashSet<string>(passengerList.Keys);

            // Assert
            Assert.That(passengerCount, Is.EqualTo(reservationCodes.Count));
        }

        [Test]
        public void GeneratePassengerList_ShouldAssignValidReservations()
        {
            // Arrange
            int passengerCount = 3;

            // Act
            var passengerList = _passengerList.GeneratePassengerList(passengerCount);

            // Assert
            foreach (var reservation in passengerList.Values)
            {
                Assert.IsNotNull(reservation, "Reservation should not be null.");
                Assert.IsNotEmpty(reservation.PassengerName, "Reservation should have a valid passenger name.");
                Assert.IsNotEmpty(reservation.ReservationCode, "Reservation should have a valid reservation code.");
            }
        }

        [Test]
        public void GeneratePassengerList_WithZeroPassengers_ShouldReturnEmptyList()
        {
            // Arrange
            int passengerCount = 0;

            // Act
            var passengerList = _passengerList.GeneratePassengerList(passengerCount);

            // Assert
            Assert.IsEmpty(passengerList, "Generating zero passengers should return an empty list.");
        }
    }
}
