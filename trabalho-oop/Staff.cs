using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.IO;

namespace trabalho_oop
{
    public class Staff : Person, IStorable
    {
        private static Logger _logger;
        
        public string staffCode { get; set; }
        public string password { get; set; }

        // Converts the object to JSON with exception handling
        public string ConvertToJson()
        {
            try
            {
                return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (JsonException ex)
            {
                _logger.Error($"Failed to serialize Staff object: {ex.Message}");
                throw new InvalidOperationException($"Failed to convert Staff {Name} to JSON", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error during JSON serialization: {ex.Message}");
                throw;
            }
        }

        public EntityType GetEntityType() => EntityType.Staff;
        public string GetIdentifier() => staffCode;

        // Generates a random staff code with exception handling
        private string GenerateStaffCode()
        {
            try
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var staffCode = new char[6];

                for (int i = 0; i < staffCode.Length; i++)
                {
                    staffCode[i] = chars[random.Next(chars.Length)];
                }

                string generatedCode = new string(staffCode);
                
                if (string.IsNullOrEmpty(generatedCode))
                {
                    throw new InvalidOperationException("Generated staff code is empty");
                }

                return generatedCode;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to generate staff code: {ex.Message}");
                throw new InvalidOperationException("Failed to generate staff code", ex);
            }
        }

        // Hashes the password with exception handling
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
                _logger.Error($"Cryptographic error while hashing password: {ex.Message}");
                throw new InvalidOperationException("Failed to hash password", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error while hashing password: {ex.Message}");
                throw;
            }
        }

        // Set and hash the password with exception handling
        public void SetPassword(string password)
        {
            try
            {
                this.password = HashPassword(password);
                _logger.Info($"Password set for staff member: {staffCode}.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to set password for staff member {staffCode}: {ex.Message}");
                throw new InvalidOperationException($"Failed to set password for staff member {staffCode}", ex);
            }
        }

        // Constructor with exception handling
        public Staff(Logger logger)
        {
            try
            {
                _logger = logger;
                this.staffCode = GenerateStaffCode();
                _logger.Info($"New staff created with staff code: {staffCode}.");
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to create new staff member: {ex.Message}");
                throw new InvalidOperationException("Failed to create new staff member", ex);
            }
        }
    }
}