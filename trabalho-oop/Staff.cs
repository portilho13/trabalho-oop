using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace trabalho_oop
{
    public class Staff: Person, IStorable
    {
        private static Logger logger = Logger.Instance("./fms/logs/app.log");  // Logger instance for logging

        public string staffCode { get; set; }
        public string password { get; set; }

        // Converts the object to JSON
        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        public EntityType GetEntityType() => EntityType.Staff;
        public string GetIdentifier() => staffCode;

        // Generates a random staff code
        private string GenerateStaffCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var staffCode = new char[6];

            for (int i = 0; i < staffCode.Length; i++)
            {
                staffCode[i] = chars[random.Next(chars.Length)];
            }

            return new string(staffCode);
        }

        // Hashes the password
        private string HashPassword(string password)
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

        // Set and hash the password, then log the action
        public void SetPassword(string password)
        {
            this.password = HashPassword(password);
            logger.Info($"Password set for staff member: {staffCode}.");
        }

        // Constructor to create a staff member with a unique staff code
        public Staff()
        {
            this.staffCode = GenerateStaffCode();
            logger.Info($"New staff created with staff code: {staffCode}.");
        }

        ~Staff() {}
    }
}
