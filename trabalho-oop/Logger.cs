using System;
using System.IO;

namespace trabalho_oop
{
    public class Logger
    {
        private static Logger _instance;
        private string _logFilePath;
        
        // Private constructor to prevent instantiation
        private Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
            
            // Ensure the log file exists, or create it
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose(); // Create the log file if it doesn't exist
            }
        }

        // Singleton instance getter, now requires a file path
        public static Logger Instance(string logFilePath = null)
        {
            if (_instance == null)
            {
                if (logFilePath == null)
                {
                    throw new ArgumentException("Log file path must be provided.");
                }
                _instance = new Logger(logFilePath);
            }
            return _instance;
        }

        // Method to write logs to both console and file
        private void Log(string message, string logLevel)
        {
            // Log to console
            Console.WriteLine($"{logLevel}: {message}");

            // Log to file
            File.AppendAllText(_logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logLevel}: {message}{Environment.NewLine}");
        }

        // Logging methods
        public void Info(string message)
        {
            Log(message, "INFO");
        }

        public void Warn(string message)
        {
            Log(message, "WARN");
        }

        public void Error(string message)
        {
            Log(message, "ERROR");
        }
    }
}