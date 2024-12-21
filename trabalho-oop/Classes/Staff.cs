//-----------------------------------------------------------------
//    <copyright file="Staff.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// The Staff class represents a staff member in the system, handling
    /// their personal details, staff code, and password. It also provides
    /// functionality for securely hashing passwords and converting the object
    /// to a JSON format for persistence or communication purposes.
    /// </summary>
    public class Staff : Person, IStorable
    {
        // Logger instance for logging activities related to staff
        [NonSerialized]
        private static ILogger _logger;
        // The unique staff code assigned to the staff member
        public string StaffCode { get; set; }

        // The hashed password for the staff member
        public string Password { get; set; }

        /// <summary>
        /// Converts the Staff object to a JSON string for serialization.
        /// Handles potential serialization errors and logs them.
        /// </summary>
        /// <returns>Serialized JSON string of the Staff object</returns>
        public string ConvertToJson()
        {
            try
            {
                // Serializing the Staff object to JSON with indented formatting
                return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (JsonException ex)
            {
                // Log any errors encountered during serialization
                _logger.Error($"Failed to serialize Staff object: {ex.Message}");
                // Throwing a custom exception with additional context
                throw new InvalidOperationException($"Failed to convert Staff {Name} to JSON", ex);
            }
            catch (Exception ex)
            {
                // Handling unexpected errors during serialization
                _logger.Error($"Unexpected error during JSON serialization: {ex.Message}");
                throw;
            }
        }
        
        private void ValidateConstructorParameters(string name, string email, string password)
        {
            // Ensure company is not empty or whitespace
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty or whitespace.", nameof(name));

            // Ensure registration is not empty or whitespace and within valid length
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty or whitespace.", nameof(email));

            // Ensure model is not empty or whitespace
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(password));
        }

        /// <summary>
        /// Returns the entity type for the staff, which is 'Staff'.
        /// </summary>
        /// <returns>EntityType.Staff</returns>
        public EntityType GetEntityType() => EntityType.Staff;

        /// <summary>
        /// Gets the unique identifier for the staff, which is the staff code.
        /// </summary>
        /// <returns>Staff's unique staff code</returns>
        public string GetIdentifier() => StaffCode;

        /// <summary>
        /// Constructor for the Staff class that initializes the staff code and logs the creation event.
        /// Handles errors during object creation and logs them.
        /// </summary>
        public Staff(string name, string email, string password, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
            ValidateConstructorParameters(name, email, password);
            Name = name;
            Email = email;
            Password = PasswordUtility.HashPassword(password);
            StaffCode = NumberGenerator.GenerateRandomNumber();
            _logger.Info($"New staff created with staff code: {StaffCode}.");
        }
        
        public Staff() {}
    }
}