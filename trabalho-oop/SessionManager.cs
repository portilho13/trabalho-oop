using System.Security.Cryptography;
using System.Text;

namespace trabalho_oop;

public class SessionManager
{
    private Session activeSession;
    
    private List<Staff> _staff = new List<Staff>();
    private List<Passanger> _passangers = new List<Passanger>();
    public FMS fms { private get; set; }

    public bool LoginStaff(string staffCode, string password)
    {
        Staff staff = _staff.Find(s => s.staff_code == staffCode && s.password == CreateHashPassword(password));
        if (staff != null)
        {
            activeSession = new Session(staff);
            return true;
        }
        return false;
    }

    private string CreateHashPassword(string password)
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

    public void Load()
    {
        _staff = fms.ReadStaffFromFolder();
    }

    public bool LoginPassanger(string email, string password)
    {
        Passanger passanger = _passangers.Find(s => s.Email == email && s.password == password);
        if (passanger != null)
        {
            activeSession = new Session(passanger);
            return true;
        }

        return false;
    }
    
    public void Logout()
    {
        activeSession = null;
    }

    public bool IsAuthenticated()
    {
        return activeSession != null;
    }

    public Person GetLoggedInPerson()
    {
        return activeSession?.LoggedInPerson;
    }

    public void RegisterStaff(string name, string email, string password)
    {
        Staff staff = _staff.Find(s => s.Email == email);
        if (staff != null)
        {
            return;
        }
        Staff newStaff = new Staff();
        newStaff.Name = name;
        newStaff.Email = email;
        newStaff.SetPassword(password);
        
        
        _staff.Add(newStaff);
    }

    public void Save()
    {
        fms.SaveStaff(_staff);
    }
    
    public void DisplayStaffList()
    {
        foreach (Staff staff in _staff)
        {
            Console.WriteLine(staff.Name);
            Console.WriteLine(staff.Email);
            Console.WriteLine(staff.staff_code);
        }
    }

}