//-----------------------------------------------------------------
//    <copyright file="FMS.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// The FMS (Flight Management System) class is a singleton that manages files and folders related to flights, aircraft, staff, passengers, and other related entities.
    /// </summary>
    public class FMS
    {
        // Lazy initialization of the FMS singleton instance
        private static readonly Lazy<FMS> _instance = new Lazy<FMS>(() => new FMS());

        // Singleton instance to provide global access
        public static FMS Instance => _instance.Value;

        // Paths for the main folders related to flights, aircraft, staff, and passengers
        public static readonly string MainFolderPath = "./fms"; // Default path for FMS
        public static readonly string FlightFolderPath = Path.Combine(MainFolderPath, "flights");
        public static readonly string AirplaneFolderPath = Path.Combine(MainFolderPath, "airplane");
        public static readonly string StaffFolderPath = Path.Combine(MainFolderPath, "staff");
        public static readonly string PassengerFolderPath = Path.Combine(MainFolderPath, "passenger");
        public static readonly string AirportFolderPath = Path.Combine(MainFolderPath, "airports");

        // File names for storing names and surnames
        private readonly string NamesFile = "../../../nomes.txt";
        private readonly string SurnamesFile = "../../../apelidos.txt";

        // List of folders to be created within the FMS directory
        private List<string> Folders = new List<string>
        {
            FlightFolderPath,
            AirplaneFolderPath,
            StaffFolderPath,
            PassengerFolderPath,
            AirportFolderPath,
        };

        // Private constructor for singleton pattern
        private FMS() { }

        /// <summary>
        /// Checks if a file exists at the given file path.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        private bool DoesFileExist(string filePath)
        {
            try
            {
                return File.Exists(filePath);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is PathTooLongException)
            {
                throw new ArgumentException($"Invalid file path: {filePath}", ex);
            }
        }

        /// <summary>
        /// Checks if a folder exists at the given file path.
        /// </summary>
        /// <param name="filePath">The path to the folder.</param>
        /// <returns>True if the folder exists, false otherwise.</returns>
        private bool DoesFolderExists(string filePath)
        {
            try
            {
                return Directory.Exists(filePath);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is PathTooLongException)
            {
                throw new ArgumentException($"Invalid folder path: {filePath}", ex);
            }
        }

        /// <summary>
        /// Converts the current instance of the FMS object to a JSON string.
        /// </summary>
        /// <returns>The JSON string representing the FMS object.</returns>
        private string ConvertToJson()
        {
            try
            {
                return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to serialize object to JSON", ex);
            }
        }

        /// <summary>
        /// Writes the provided JSON string to a specified file path.
        /// </summary>
        /// <param name="filePath">The path to the file where the JSON should be written.</param>
        /// <param name="jsonString">The JSON string to write to the file.</param>
        public void WriteJsonToFile(string filePath, string jsonString)
        {
            try
            {
                // Ensures the folder exists before writing the file
                string folderPath = Path.GetDirectoryName(filePath);
                if (!DoesFolderExists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // Creates the file if it doesn't exist
                if (!DoesFileExist(filePath))
                {
                    CreateFile(filePath);
                }

                // Write the JSON to the file
                File.WriteAllText(filePath, jsonString);
                Console.WriteLine("JSON written successfully");
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to write JSON to file: {filePath}", ex);
            }
        }

        /// <summary>
        /// Creates a file at the given filename if it does not already exist.
        /// </summary>
        /// <param name="filename">The name of the file to create.</param>
        private void CreateFile(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    using FileStream fs = File.Create(filename);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to create file: {filename}", ex);
            }
        }

        /// <summary>
        /// Creates a folder at the given foldername if it does not already exist.
        /// </summary>
        /// <param name="foldername">The name of the folder to create.</param>
        private void CreateFolder(string foldername)
        {
            try
            {
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to create folder: {foldername}", ex);
            }
        }

        /// <summary>
        /// Initializes the FMS system by creating the main directory and its subdirectories.
        /// </summary>
        public void Start()
        {
            try
            {
                // Creates the main folder if it doesn't exist
                if (!Directory.Exists(MainFolderPath))
                {
                    Directory.CreateDirectory(MainFolderPath);
                }

                // Creates the subdirectories for flights, aircraft, staff, and passengers
                foreach (string folder in Folders)
                {
                    CreateFolder(folder);
                }

                Console.WriteLine("FMS Started Successfully");
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new InvalidOperationException("Failed to initialize FMS system", ex);
            }
        }

        /// <summary>
        /// Reads the contents of a JSON file and returns it as a string.
        /// </summary>
        /// <param name="filePath">The path to the file to read from.</param>
        /// <returns>The contents of the file as a string.</returns>
        public string ReadFromJson(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex) when (ex is FileNotFoundException)
            {
                throw new FileNotFoundException($"JSON file not found: {filePath}", ex);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to read JSON file: {filePath}", ex);
            }
        }

        /// <summary>
        /// Deletes an airplane file from the system based on the airplane's registration.
        /// </summary>
        /// <param name="airplane">The airplane object to delete.</param>
        public void DeleteAirplane(Airplane airplane)
        {
            try
            {
                string airplanePath = Path.Combine(AirplaneFolderPath, airplane.Registration + ".json");
                if (File.Exists(airplanePath))
                {
                    File.Delete(airplanePath);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to delete airplane file for registration: {airplane.Registration}", ex);
            }
        }
        
        /// <summary>
        /// Deletes an airplane file from the system based on the airplane's registration.
        /// </summary>
        /// <param name="airplane">The airplane object to delete.</param>
        public void DeleteAirport(Airport airport)
        {
            try
            {
                string airportPath = Path.Combine(AirportFolderPath, airport.ICAO + ".json");
                if (File.Exists(airportPath))
                {
                    File.Delete(airportPath);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to delete airport file for ICAO: {airport.ICAO}", ex);
            }
        }
        
        /// <summary>
        /// Deletes a flight file from the system based on the flight number.
        /// </summary>
        /// <param name="flight">The flight object to delete.</param>
        public void DeleteFlight(Flight flight)
        {
            try
            {
                string flightPath = Path.Combine(FlightFolderPath, flight.Number + ".json");
                if (File.Exists(flightPath))
                {
                    File.Delete(flightPath);
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to delete flight file for number: {flight.Number}", ex);
            }
        }

        /// <summary>
        /// Reads all airplane files from the aircraft folder.
        /// </summary>
        /// <returns>An array of file paths for all airplane files in the folder.</returns>
        public string[] ReadAirplaneFromFolder()
        {
            try
            {
                return Directory.GetFiles(AirplaneFolderPath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException("Failed to read airplane files from folder", ex);
            }
        }
        
        /// <summary>
        /// Reads all flight files from the flight folder.
        /// </summary>
        /// <returns>An array of file paths for all flight files in the folder.</returns>
        public string[] ReadFlightsFromFolder()
        {
            try
            {
                return Directory.GetFiles(FlightFolderPath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException("Failed to read flights files from folder", ex);
            }
        }
        
        /// <summary>
        /// Reads all airports files from the airports folder.
        /// </summary>
        /// <returns>An array of file paths for all airports files in the folder.</returns>
        public string[] ReadAirportsFromFolder()
        {
            try
            {
                return Directory.GetFiles(AirportFolderPath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException("Failed to read Airports files from folder", ex);
            }
        }

        /// <summary>
        /// Checks if a staff member exists based on their staff code.
        /// </summary>
        /// <param name="staffCode">The staff code.</param>
        /// <returns>True if the staff member exists, false otherwise.</returns>
        private bool DoesStaffExist(string staffCode)
        {
            try
            {
                return File.Exists(Path.Combine(StaffFolderPath, staffCode + ".json"));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is PathTooLongException)
            {
                throw new ArgumentException($"Invalid staff code: {staffCode}", ex);
            }
        }
        
        /// <summary>
        /// Reads all staff files from the staff folder.
        /// </summary>
        /// <returns>An array of file paths for all staff files in the folder.</returns>
        public string[] ReadStaffFromFolder()
        {
            try
            {
                return Directory.GetFiles(StaffFolderPath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException("Failed to read staff files from folder", ex);
            }
        }
        
        /// <summary>
        /// Reads all passenger files from the passenger folder.
        /// </summary>
        /// <returns>An array of file paths for all passenger files in the folder.</returns>
        public string[] ReadPassengersFromFolder()
        {
            try
            {
                return Directory.GetFiles(PassengerFolderPath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException("Failed to read passenger files from folder", ex);
            }
        }

        /// <summary>
        /// Retrieves the list of passenger names from the names file.
        /// </summary>
        /// <returns>A list of passenger names.</returns>
        public List<string> GetPassengerNames()
        {
            try
            {
                List<string> namesList = new List<string>();
                string[] names = File.ReadAllLines(NamesFile);
                namesList.AddRange(names);
                return namesList;
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException($"Names file not found: {NamesFile}", ex);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to read names file: {NamesFile}", ex);
            }
        }

        /// <summary>
        /// Retrieves the list of passenger surnames from the surnames file.
        /// </summary>
        /// <returns>A list of passenger surnames.</returns>
        public List<string> GetPassengerSurnames()
        {
            try
            {
                List<string> surnamesList = new List<string>();
                string[] surnames = File.ReadAllLines(SurnamesFile);
                surnamesList.AddRange(surnames);
                return surnamesList;
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException($"Surnames file not found: {SurnamesFile}", ex);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to read surnames file: {SurnamesFile}", ex);
            }
        }

        /// <summary>
        /// Retrieves the folder path for a given entity type.
        /// </summary>
        /// <param name="entity">The entity type.</param>
        /// <returns>The folder path for the entity type.</returns>
        private string GetEntityFolderPath(EntityType entity)
        {
            try
            {
                switch (entity)
                {
                    case EntityType.Airplane:
                        return AirplaneFolderPath;
                    case EntityType.Passenger:
                        return PassengerFolderPath;
                    case EntityType.Flight:
                        return FlightFolderPath;
                    case EntityType.Staff:
                        return StaffFolderPath;
                    case EntityType.Airport:
                        return AirportFolderPath;
                    default:
                        throw new ArgumentException($"Invalid entity type: {entity}", nameof(entity));
                }
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                throw new ArgumentException("Failed to get entity folder path", ex);
            }
        }

        /// <summary>
        /// Saves an entity (such as airplane, flight, etc.) by converting it to JSON and writing it to the corresponding folder.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        public void Save(IStorable entity)
        {
            try
            {
                string json = entity.ConvertToJson();
                string name = entity.GetIdentifier();
                string path = GetEntityFolderPath(entity.GetEntityType());
                string fullPath = Path.Combine(path, name + ".json");
                WriteJsonToFile(fullPath, json);
            }
            catch (Exception ex) when (ex is ArgumentNullException)
            {
                throw new ArgumentNullException("Entity cannot be null", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write json to file: {ex.Message}");
                throw new InvalidOperationException($"Failed to save entity {entity.GetEntityType()}", ex);
            }
        }
    }
}