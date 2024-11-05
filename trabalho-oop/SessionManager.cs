namespace trabalho_oop;

public class SessionManager
{
    private Session activeSession;
    
    private List<Staff> _staff = new List<Staff>();

    public bool Login(string staffCode, string password, bool isStaff)
    {
        if (isStaff)
        {
            Staff staff = _staff.Find(s => s.staff_code == staffCode && s.password == password);
            if (staff != null)
            {
                activeSession = new Session(staff);
                return true;
            }
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
    
    
    
}