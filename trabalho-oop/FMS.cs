using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace trabalho_oop
{
    public class FMS
    {
        public string MainFolderPath = @"./fms"; // Default path for FMS

        public string FlightFolderPath;
        
        public string AircraftFolderPath;

        public string StaffFolderPath;

        private List<string> Folders;

        public FMS()
        {
            FlightFolderPath = Path.Combine(MainFolderPath, "flights");
            AircraftFolderPath = Path.Combine(MainFolderPath, "aircraft");
            StaffFolderPath = Path.Combine(MainFolderPath, "staff");
            Folders = new List<string>
            {
                FlightFolderPath,
                AircraftFolderPath,
                StaffFolderPath
            };
        }

        private bool DoesFileExist(string filePath) => File.Exists(filePath);

        private bool DoesFolderExists(string filePath) => File.Exists(filePath);
        
        private string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });


        public void WriteJsonToFile(string filePath, string jsonString)
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

        private void CreateFile(string filename)
        {
            if (!File.Exists(filename))
            {
                using FileStream fs = File.Create(filename);
            }
        }

        private void CreateFolder(string foldername)
        {
            if (!Directory.Exists(foldername))
            {
                Directory.CreateDirectory(foldername);
            }
        }

        public void Start()
        {
            if (!Directory.Exists(MainFolderPath))
            {
                Directory.CreateDirectory(MainFolderPath);
            }

            foreach (string folder in Folders)
            {
                CreateFolder(folder); // This should create the "flights" folder
            };

            Console.WriteLine("FMS Started Successfully");
        }

        public string ReadFromJson(string filePath) => File.ReadAllText(filePath);
        
        public void SaveFlight(Flight flight)
        {
            string json = flight.ConvertToJson();

            string flighFile = flight.Number + ".json";

            string path = Path.Combine(FlightFolderPath, flighFile);

            WriteJsonToFile(path, json);
        }

        public void SaveAirplane(Airplane airplane)
        {
            string json = airplane.ConvertToJson();
            string reg = airplane.Registration + ".json";
            string path = Path.Combine(this.AircraftFolderPath, reg);
            this.WriteJsonToFile(path, json);
        }

        public void DeleteAirplane(Airplane airplane)
        {
            string airplanePath = Path.Combine(AircraftFolderPath, airplane.Registration + ".json");
            if (File.Exists(airplanePath))
            {
                File.Delete(airplanePath);
            }
        }
        
        public string[] ReadAirplaneFromFolder()
        {
            return Directory.GetFiles(AircraftFolderPath);
        }

        public void SaveStaff(List<Staff> staffList)
        {

            foreach (Staff staff in staffList)
            {
                string json = staff.ConvertToJson();
                string code = staff.staff_code + ".json";
                string path = Path.Combine(this.StaffFolderPath, code);
                WriteJsonToFile(path, json);
            }
        }
        
        private bool DoesStaffExist(string staffCode) => File.Exists(Path.Combine(StaffFolderPath, staffCode + ".json"));

        public List<Staff> ReadStaffFromFolder()
        {
            List<Staff> staffList = new List<Staff>();
        
            // Get only .json files in the folder
            string[] files = Directory.GetFiles(StaffFolderPath, "*.json");

            foreach (string file in files)
            {
                if (!DoesStaffExist(file))
                {
                    try
                    {
                        // Read JSON content
                        string json = File.ReadAllText(file);

                        // Skip if file is empty
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
                        // Handle JSON-specific exceptions
                        Console.WriteLine($"Failed to deserialize JSON in file {file}: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        // Handle any other exceptions (e.g., file read issues)
                        Console.WriteLine($"Error reading file {file}: {ex.Message}");
                    }
                }
            }

            return staffList;
        }
        ~FMS() { }
    }
}
