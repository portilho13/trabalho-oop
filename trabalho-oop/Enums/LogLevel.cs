//-----------------------------------------------------------------
//    <copyright file="LogLevel.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

namespace trabalho_oop
{
    /// <summary>
    /// Enum representing the different log levels for logging messages.
    /// Used to categorize log entries by their severity or purpose.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Informational log level. Used for general information about the program's operation.
        /// </summary>
        Info,

        /// <summary>
        /// Warning log level. Used to indicate a potential issue or something unusual but not necessarily an error.
        /// </summary>
        Warning,

        /// <summary>
        /// Error log level. Used to indicate a significant problem or failure in the program.
        /// </summary>
        Error,

        /// <summary>
        /// Debug log level. Used for detailed information, typically for debugging purposes, and might include verbose or fine-grained logs.
        /// </summary>
        Debug
    }
}