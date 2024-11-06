using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace trabalho_oop;

public class Staff: Person
{
    public string staff_code { get; set; }
    
    public string password { get; set; }
    
    public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

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

    public void SetPassword(string password)
    {
        password = HashPassword(password);
        this.password = password;
    }

    public Staff()
    {
        this.staff_code = GenerateStaffCode();
    }
    
    ~Staff() {}
}