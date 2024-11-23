//-----------------------------------------------------------------
//    <copyright file="Logger.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System;
using System.IO;

namespace trabalho_oop
{
    /// <summary>
    /// The Logger class is responsible for logging messages to both the console
    /// and a specified log file. It supports different log levels (INFO, WARN, ERROR).
    /// </summary>
    public class Logger: ILogger
    {
        private readonly string _logFilePath;  // Path where the log file is stored

        /// <summary>
        /// Initializes a new instance of the Logger class with a specified log file path.
        /// Ensures the directory exists and creates the log file if it does not exist.
        /// </summary>
        /// <param name="logFilePath">The path to the log file.</param>
        public Logger(string logFilePath)
        {
            // Ensure the log file path is provided and not empty or whitespace
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                throw new ArgumentException("Log file path must be provided.", nameof(logFilePath));
            }

            _logFilePath = logFilePath;

            // Ensure the directory for the log file exists, or create it
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

            // Create the log file if it does not exist
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose(); // Dispose the file stream after creating the file
            }
        }

        /// <summary>
        /// Logs a message with the specified log level to both the console and the log file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logLevel">The log level (e.g., INFO, WARN, ERROR).</param>
        private void Log(string message, string logLevel)
        {
            // Log the message to the console
            Console.WriteLine($"{logLevel}: {message}");

            // Log the message to the file with a timestamp
            File.AppendAllText(_logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {logLevel}: {message}{Environment.NewLine}");
        }

        /// <summary>
        /// Logs an informational message to both the console and the log file.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        public void Info(string message)
        {
            Log(message, "INFO");
        }

        /// <summary>
        /// Logs a warning message to both the console and the log file.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void Warn(string message)
        {
            Log(message, "WARN");
        }

        /// <summary>
        /// Logs an error message to both the console and the log file.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void Error(string message)
        {
            Log(message, "ERROR");
        }
    }
}