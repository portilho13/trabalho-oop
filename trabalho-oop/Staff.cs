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
        private static Logger _logger;

        // The unique staff code assigned to the staff member
        public string staffCode { get; set; }

        // The hashed password for the staff member
        public string password { get; set; }

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

        /// <summary>
        /// Returns the entity type for the staff, which is 'Staff'.
        /// </summary>
        /// <returns>EntityType.Staff</returns>
        public EntityType GetEntityType() => EntityType.Staff;

        /// <summary>
        /// Gets the unique identifier for the staff, which is the staff code.
        /// </summary>
        /// <returns>Staff's unique staff code</returns>
        public string GetIdentifier() => staffCode;

        /// <summary>
        /// Generates a random 6-character staff code consisting of letters and numbers.
        /// Handles errors in staff code generation and logs them.
        /// </summary>
        /// <returns>Generated staff code</returns>
        private string GenerateStaffCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var staffCode = new char[6];

            // Generating the staff code by randomly selecting characters
            for (int i = 0; i < staffCode.Length; i++)
            {
                staffCode[i] = chars[random.Next(chars.Length)];
            }

            string generatedCode = new string(staffCode);
    
            // Simple validation can be a direct throw
            if (string.IsNullOrEmpty(generatedCode))
            {
                throw new InvalidOperationException("Generated staff code is empty");
            }

            return generatedCode;
        }

        /// <summary>
        /// Hashes the provided password using SHA256 for secure storage.
        /// Handles errors during hashing and logs them.
        /// </summary>
        /// <param name="password">Plain text password to hash</param>
        /// <returns>SHA256 hash of the password</returns>
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
            }

            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashBytes = sha256Hash.ComputeHash(passwordBytes);

                    // Converting the byte array to a hex string
                    StringBuilder hashString = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        hashString.Append(b.ToString("x2"));
                    }

                    return hashString.ToString();
                }
            }
            catch (CryptographicException ex)
            {
                // Logging cryptographic errors encountered during hashing
                _logger.Error($"Cryptographic error while hashing password: {ex.Message}");
                throw new InvalidOperationException("Failed to hash password", ex);
            }
            catch (Exception ex)
            {
                // Handling unexpected errors during password hashing
                _logger.Error($"Unexpected error while hashing password: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sets the staff member's password by hashing the provided password.
        /// Logs the password set operation.
        /// </summary>
        /// <param name="password">Plain text password to set</param>
        public void SetPassword(string password)
        {
            this.password = HashPassword(password);
            _logger.Info($"Password set for staff member: {staffCode}.");
        }

        /// <summary>
        /// Constructor for the Staff class that initializes the staff code and logs the creation event.
        /// Handles errors during object creation and logs them.
        /// </summary>
        public Staff(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
            staffCode = GenerateStaffCode();
            _logger.Info($"New staff created with staff code: {staffCode}.");
        }
    }
}