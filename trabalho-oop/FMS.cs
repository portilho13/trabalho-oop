using System;
using System.IO;
using System.Collections.Generic;

namespace trabalho_oop
{
    public class FMS
    {
        public string MainFolderPath = @"./fms"; // Default path for FMS

        public string FlightFolderPath;
        
        public string AircraftFolderPath;

        private List<string> Folders;

        public FMS()
        {
            FlightFolderPath = Path.Combine(MainFolderPath, "flights");
            AircraftFolderPath = Path.Combine(MainFolderPath, "aircraft");
            Folders = new List<string>
            {
                FlightFolderPath,
                AircraftFolderPath,
            };
        }

        private bool DoesFileExist(string filePath) => File.Exists(filePath);

        private bool DoesFolderExists(string filePath) => File.Exists(filePath);

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

        public void SaveAirplane(Airplane airplane)
        {
            string json = airplane.ConvertToJson();
            string reg = airplane.Registration + ".json";
            string path = Path.Combine(this.AircraftFolderPath, reg);
            this.WriteJsonToFile(path, json);
        }

        public void DeleteAirplane(Airplane airplane)
        {
            string airplane_path = Path.Combine(AircraftFolderPath, airplane.Registration + ".json");
            if (File.Exists(airplane_path))
            {
                File.Delete(airplane_path);
            }
        }
        ~FMS() { }
    }
}
