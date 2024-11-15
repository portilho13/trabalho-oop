using System;
using System.IO;

namespace trabalho_oop
{
    public class Logger
    {
        private readonly string _logFilePath;

        // Constructor requires a log file path to initialize
        public Logger(string logFilePath)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                throw new ArgumentException("Log file path must be provided.", nameof(logFilePath));
            }

            _logFilePath = logFilePath;

            // Ensure the log file exists, or create it
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose(); // Create the log file if it doesn't exist
            }
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