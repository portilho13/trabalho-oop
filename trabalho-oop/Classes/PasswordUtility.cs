using System.Security.Cryptography;
using System.Text;
namespace trabalho_oop
{

    public static class PasswordUtility
    {
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

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
    }
}