using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace trabalho_oop
{
    public class FMS
    {
        public static string MainFolderPath = "./fms"; // Default path for FMS
        public static string FlightFolderPath = Path.Combine(MainFolderPath, "flights");
        public static string AircraftFolderPath = Path.Combine(MainFolderPath, "aircraft");
        public static string StaffFolderPath = Path.Combine(MainFolderPath, "staff");
        public static string PassengerFolderPath = Path.Combine(MainFolderPath, "passenger");

        private string NamesFile = "nomes.txt";
        private string SurnamesFile = "apelidos.txt";

        private List<string> Folders = new List<string>
        {
            FlightFolderPath,
            AircraftFolderPath,
            StaffFolderPath
        };

        public FMS()
        {
        }

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

        public void WriteJsonToFile(string filePath, string jsonString)
        {
            try
            {
                string folderPath = Path.GetDirectoryName(filePath);
                if (!DoesFolderExists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (!DoesFileExist(filePath))
                {
                    CreateFile(filePath);
                }

                File.WriteAllText(filePath, jsonString);
                Console.WriteLine("JSON written successfully");
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException($"Failed to write JSON to file: {filePath}", ex);
            }
        }

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

        public void Start()
        {
            try
            {
                if (!Directory.Exists(MainFolderPath))
                {
                    Directory.CreateDirectory(MainFolderPath);
                }

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

        public void DeleteAirplane(Airplane airplane)
        {
            try
            {
                string airplanePath = Path.Combine(AircraftFolderPath, airplane.Registration + ".json");
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

        public string[] ReadAirplaneFromFolder()
        {
            try
            {
                return Directory.GetFiles(AircraftFolderPath);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException("Failed to read airplane files from folder", ex);
            }
        }

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

        public List<Staff> ReadStaffFromFolder()
        {
            List<Staff> staffList = new List<Staff>();
            
            try
            {
                string[] files = Directory.GetFiles(StaffFolderPath, "*.json");

                foreach (string file in files)
                {
                    if (!DoesStaffExist(file))
                    {
                        try
                        {
                            string json = File.ReadAllText(file);

                            if (string.IsNullOrWhiteSpace(json))
                            {
                                Console.WriteLine($"Skipped empty file: {file}");
                                continue;
                            }

                            Staff staff = JsonSerializer.Deserialize<Staff>(json, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        
                            if (staff != null)
                            {
                                staffList.Add(staff);
                            }
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Failed to deserialize JSON in file {file}: {ex.Message}");
                            // Continue processing other files
                            continue;
                        }
                        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                        {
                            Console.WriteLine($"Error reading file {file}: {ex.Message}");
                            // Continue processing other files
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                throw new IOException("Failed to read staff folder", ex);
            }

            return staffList;
        }

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

        private string GetEntityFolderPath(EntityType entity)
        {
            try
            {
                switch (entity)
                {
                    case EntityType.Airplane:
                        return AircraftFolderPath;
                    case EntityType.Passenger:
                        return PassengerFolderPath;
                    case EntityType.Flight:
                        return FlightFolderPath;
                    case EntityType.Staff:
                        return StaffFolderPath;
                    default:
                        throw new ArgumentException($"Invalid entity type: {entity}", nameof(entity));
                }
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                throw new ArgumentException("Failed to get entity folder path", ex);
            }
        }

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
                throw new InvalidOperationException("Failed to save entity", ex);
            }
        }

        ~FMS() { }
    }
}