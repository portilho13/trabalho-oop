using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace trabalho_oop
{
    public class SessionManager
    {
        private Session activeSession;

        private List<Staff> _staff = new List<Staff>();
        private List<Passenger> _passengers = new List<Passenger>();
        public FMS fms { private get; set; }
        
        private static Logger logger = Logger.Instance("./fms/logs/app.log");  // Logger instance for logging

        // Login method for Staff
        public bool LoginStaff(string staffCode, string password)
        {
            Staff staff = _staff.Find(s => s.staffCode == staffCode && s.password == CreateHashPassword(password));
            if (staff != null)
            {
                activeSession = new Session(staff);
                logger.Info($"Staff login successful: {staff.Name} ({staff.Email})");
                return true;
            }
            logger.Warn($"Staff login failed: Invalid credentials for {staffCode}");
            return false;
        }

        // Create hashed password for staff login validation
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

        // Load staff data from FMS
        public void Load()
        {
            _staff = fms.ReadStaffFromFolder();
            logger.Info("Staff data loaded from FMS.");
        }

        // Login method for Passenger
        public bool LoginPassenger(string email, string password)
        {
            Passenger passenger = _passengers.Find(s => s.Email == email && s.password == password);
            if (passenger != null)
            {
                activeSession = new Session(passenger);
                logger.Info($"Passenger login successful: {passenger.Name} ({passenger.Email})");
                return true;
            }
            logger.Warn($"Passenger login failed: Invalid credentials for {email}");
            return false;
        }

        // Logout method
        public void Logout()
        {
            logger.Info("Logging out.");
            activeSession = null;
        }

        // Check if there is an active session
        public bool IsAuthenticated()
        {
            return activeSession != null;
        }

        // Get the currently logged in user (Staff or Passenger)
        public Person GetLoggedInPerson()
        {
            return activeSession?.LoggedInPerson;
        }

        // Register new staff
        public void RegisterStaff(string name, string email, string password)
        {
            Staff staff = _staff.Find(s => s.Email == email);
            if (staff != null)
            {
                logger.Warn($"Staff registration failed: Email {email} already exists.");
                return;
            }
            Staff newStaff = new Staff
            {
                Name = name,
                Email = email
            };
            newStaff.SetPassword(password);

            _staff.Add(newStaff);
            logger.Info($"New staff registered: {name} ({email})");
        }

        // Save staff and passenger data to FMS
        public void Save()
        {
            foreach (Staff staff in _staff)
                fms.Save(staff);

            foreach (Passenger passenger in _passengers)
                fms.Save(passenger);

            logger.Info("Staff and Passenger data saved to FMS.");
        }

        // Display staff list
        public void DisplayStaffList()
        {
            foreach (Staff staff in _staff)
            {
                Console.WriteLine(staff.Name);
                Console.WriteLine(staff.Email);
                Console.WriteLine(staff.staffCode);
            }
        }
    }
}
