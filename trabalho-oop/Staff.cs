namespace trabalho_oop;

public class Staff: Person
{
    public string staff_code;
    
    public string password;

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

    public Staff()
    {
        this.staff_code = GenerateStaffCode();
    }

    public void SetPassword(string password)
    {
        this.password = password;
    }
    
    ~Staff() {}
}