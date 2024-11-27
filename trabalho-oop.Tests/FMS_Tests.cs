using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace trabalho_oop.Tests
{
    [TestFixture]
    public class FMS_Tests
    {
        private FMS _fms;
        private TestLogger _logger;
        private readonly string _testJsonContent = "{\"test\": \"data\"}";

        [SetUp]
        public void SetUp()
        {
            // Initialize the singleton instance
            _fms = FMS.Instance;
            _logger = new TestLogger();
            
            // Create temporary test paths
            if (Directory.Exists(FMS.MainFolderPath))
                Directory.Delete(FMS.MainFolderPath, true);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up after tests
            if (Directory.Exists(FMS.MainFolderPath))
                Directory.Delete(FMS.MainFolderPath, true);
        }
        
        [Test]
        public void TestStart_CreatesMainAndSubFolders()
        {
            // Act
            _fms.Start();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(Directory.Exists(FMS.MainFolderPath), "Main folder should exist");
                Assert.That(Directory.Exists(FMS.FlightFolderPath), "Flight folder should exist");
                Assert.That(Directory.Exists(FMS.AirplaneFolderPath), "Aircraft folder should exist");
                Assert.That(Directory.Exists(FMS.StaffFolderPath), "Staff folder should exist");
                Assert.That(Directory.Exists(FMS.PassengerFolderPath), "Passenger folder should exist");
            });
            _logger.Info("All required folders were created successfully");
        }

        [Test]
        public void Instance_ShouldReturnSameInstance()
        {
            // Arrange & Act
            var instance1 = FMS.Instance;
            var instance2 = FMS.Instance;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(instance1, Is.Not.Null);
                Assert.That(instance2, Is.Not.Null);
                Assert.That(instance1, Is.SameAs(instance2));
            });
            _logger.Info("Singleton pattern test passed");
        }

        [Test]
        public void WriteJsonToFile_ValidPath_CreatesFileWithContent()
        {
            // Arrange
            _fms.Start();
            string testFilePath = Path.Combine(FMS.MainFolderPath, "test.json");

            // Act
            _fms.WriteJsonToFile(testFilePath, _testJsonContent);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(File.Exists(testFilePath), "File should exist");
                Assert.That(File.ReadAllText(testFilePath), Is.EqualTo(_testJsonContent));
            });
            _logger.Info("JSON file was written successfully");
        }

        [Test]
        public void ReadFromJson_ExistingFile_ReturnsContent()
        {
            // Arrange
            _fms.Start();
            string testFilePath = Path.Combine(FMS.MainFolderPath, "test.json");
            File.WriteAllText(testFilePath, _testJsonContent);

            // Act
            string result = _fms.ReadFromJson(testFilePath);

            // Assert
            Assert.That(result, Is.EqualTo(_testJsonContent));
            _logger.Info("JSON file was read successfully");
        }

        [Test]
        public void ReadFromJson_NonExistentFile_ThrowsIOException()
        {
            // Arrange
            string nonExistentPath = Path.Combine(FMS.MainFolderPath, "nonexistent.json");

            // Act & Assert
            Assert.Throws<IOException>(() => _fms.ReadFromJson(nonExistentPath));
            _logger.Info("IOException was thrown as expected");
        }

        [Test]
        public void DeleteAirplane_ExistingAirplane_RemovesFile()
        {
            // Arrange
            _fms.Start();
            var airplane = new Airplane { Registration = "TEST123" };
            string airplanePath = Path.Combine(FMS.AirplaneFolderPath, "TEST123.json");
            File.WriteAllText(airplanePath, _testJsonContent);

            // Act
            _fms.DeleteAirplane(airplane);

            // Assert
            Assert.That(File.Exists(airplanePath), Is.False);
            _logger.Info("Airplane file was deleted successfully");
        }

        [Test]
        public void DeleteFlight_ExistingFlight_RemovesFile()
        {
            // Arrange
            _fms.Start();
            var flight = new Flight { Number = "FL123" };
            string flightPath = Path.Combine(FMS.FlightFolderPath, "FL123.json");
            File.WriteAllText(flightPath, _testJsonContent);

            // Act
            _fms.DeleteFlight(flight);

            // Assert
            Assert.That(File.Exists(flightPath), Is.False);
            _logger.Info("Flight file was deleted successfully");
        }

        [Test]
        public void ReadAirplaneFromFolder_WithFiles_ReturnsFileArray()
        {
            // Arrange
            _fms.Start();
            string testPath = Path.Combine(FMS.AirplaneFolderPath, "test.json");
            File.WriteAllText(testPath, _testJsonContent);

            // Act
            string[] files = _fms.ReadAirplaneFromFolder();

            // Assert
            Assert.That(files, Is.Not.Empty);
            Assert.That(files, Does.Contain(testPath));
            _logger.Info("Airplane files were read successfully");
        }

        [Test]
        public void GetPassengerNames_ValidFile_ReturnsNamesList()
        {
            // This test assumes the names file exists
            // Act
            List<string> names = _fms.GetPassengerNames();

            // Assert
            Assert.That(names, Is.Not.Null);
            Assert.That(names, Is.Not.Empty);
            _logger.Info("Passenger names were retrieved successfully");
        }

        [Test]
        public void GetPassengerSurnames_ValidFile_ReturnsSurnamesList()
        {
            // This test assumes the surnames file exists
            // Act
            List<string> surnames = _fms.GetPassengerSurnames();

            // Assert
            Assert.That(surnames, Is.Not.Null);
            Assert.That(surnames, Is.Not.Empty);
            _logger.Info("Passenger surnames were retrieved successfully");
        }

        [Test]
        public void Save_ValidEntity_CreatesJsonFile()
        {
            // Arrange
            _fms.Start();
            var mockEntity = new MockStorableEntity();

            // Act
            _fms.Save(mockEntity);

            // Assert
            string expectedPath = Path.Combine(FMS.FlightFolderPath, "mock.json"); // Mock type in Stored in FLights Folder
            Assert.That(File.Exists(expectedPath), Is.True);
            _logger.Info("Entity was saved successfully");
        }
    }

    // Mock class for testing IStorable implementation
    public class MockStorableEntity : IStorable
    {
        public string ConvertToJson() => "{\"mock\": \"data\"}";
        public string GetIdentifier() => "mock";
        public EntityType GetEntityType() => EntityType.Flight; // Assign FLight type will store mock in FLight Folder
    }
}