using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace trabalho_oop
{
    public class SessionManager
    {
        public Session ActiveSession;
        private readonly List<Staff> _staff;
        private readonly List<Passenger> _passengers;
        private FMS _fms;

        public FMS fms
        {
            private get => _fms;
            set => _fms = value ?? throw new ArgumentNullException(nameof(value), "FMS cannot be null");
        }
        
        

        public SessionManager()
        {
            try
            {
                _staff = new List<Staff>();
                _passengers = new List<Passenger>();
                Logger.Instance().Info("SessionManager initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Instance().Error($"Failed to initialize SessionManager: {ex.Message}");
                throw new InvalidOperationException("Failed to initialize SessionManager", ex);
            }
        }

        public bool LoginStaff(string staffCode, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(staffCode))
                    throw new ArgumentException("Staff code cannot be empty", nameof(staffCode));
                
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                if (_staff == null)
                    throw new InvalidOperationException("Staff list not initialized");

                string hashedPassword = CreateHashPassword(password);
                Staff staff = _staff.Find(s => s.staffCode == staffCode && s.password == hashedPassword);
                
                if (staff != null)
                {
                    ActiveSession = new Session(staff);
                    Logger.Instance().Info($"Staff login successful: {staff.Name} ({staff.Email})");
                    return true;
                }
                
                Logger.Instance().Warn($"Staff login failed: Invalid credentials for {staffCode}");
                return false;
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                Logger.Instance().Error($"Error during staff login: {ex.Message}");
                throw new InvalidOperationException("Failed to process staff login", ex);
            }
        }

        private string CreateHashPassword(string password)
        {
            try
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
            catch (Exception ex) when (ex is not ArgumentException)
            {
                Logger.Instance().Error("Error creating password hash");
                throw new InvalidOperationException("Failed to create password hash", ex);
            }
        }

        public void Load()
        {
            try
            {
                if (_fms == null)
                    throw new InvalidOperationException("FMS not initialized");

                _staff.Clear();
                var loadedStaff = _fms.ReadStaffFromFolder();
                
                if (loadedStaff != null)
                    _staff.AddRange(loadedStaff);
                
                Logger.Instance().Info("Staff data loaded from FMS");
            }
            catch (Exception ex)
            {
                Logger.Instance().Error($"Failed to load staff data: {ex.Message}");
                throw new InvalidOperationException("Failed to load staff data from FMS", ex);
            }
        }

        public bool LoginPassenger(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email cannot be empty", nameof(email));
                
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                if (_passengers == null)
                    throw new InvalidOperationException("Passenger list not initialized");

                Passenger passenger = _passengers.Find(s => s.Email == email && s.password == password);
                
                if (passenger != null)
                {
                    ActiveSession = new Session(passenger);
                    Logger.Instance().Info($"Passenger login successful: {passenger.Name} ({passenger.Email})");
                    return true;
                }
                
                Logger.Instance().Warn($"Passenger login failed: Invalid credentials for {email}");
                return false;
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                Logger.Instance().Error($"Error during passenger login: {ex.Message}");
                throw new InvalidOperationException("Failed to process passenger login", ex);
            }
        }

        public void Logout()
        {
            try
            {
                if (ActiveSession?.LoggedInPerson != null)
                {
                    Logger.Instance().Info($"User {ActiveSession.LoggedInPerson.Name} logged out");
                }
                ActiveSession = null;
            }
            catch (Exception ex)
            {
                Logger.Instance().Error($"Error during logout: {ex.Message}");
                throw new InvalidOperationException("Failed to process logout", ex);
            }
        }

        public bool IsAuthenticated()
        {
            try
            {
                return ActiveSession != null;
            }
            catch (Exception ex)
            {
                Logger.Instance().Error($"Error checking authentication status: {ex.Message}");
                throw new InvalidOperationException("Failed to check authentication status", ex);
            }
        }

        public void DisplayStaff()
        {
            foreach (Staff staff in _staff)
            {
                Console.WriteLine(staff.ToString());
            }
        }

        public EntityType GetEntityType()
        {
            Person person = GetLoggedInPerson();
            if (person is Passenger passenger)
            {
                return EntityType.Passenger;
            }
            return EntityType.Staff;
        }

        public Person GetLoggedInPerson()
        {
            try
            {
                return ActiveSession?.LoggedInPerson;
            }
            catch (Exception ex)
            {
                Logger.Instance().Error($"Error retrieving logged in person: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve logged in person", ex);
            }
        }

        public void RegisterStaff(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be empty", nameof(name));
                
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email cannot be empty", nameof(email));
                
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                if (_staff == null)
                    throw new InvalidOperationException("Staff list not initialized");

                Staff existingStaff = _staff.Find(s => s.Email == email);
                if (existingStaff != null)
                {
                    Logger.Instance().Warn($"Staff registration failed: Email {email} already exists");
                    throw new InvalidOperationException($"Email {email} is already registered");
                }

                Staff newStaff = new Staff
                {
                    Name = name,
                    Email = email
                };
                newStaff.SetPassword(password);

                _staff.Add(newStaff);
                Logger.Instance().Info($"New staff registered: {name} ({email})");
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                Logger.Instance().Error($"Error during staff registration: {ex.Message}");
                throw new InvalidOperationException("Failed to register staff", ex);
            }
        }
        
        public void RegisterPassanger(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be empty", nameof(name));
                
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email cannot be empty", nameof(email));
                
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                if (_passengers == null)
                    throw new InvalidOperationException("Staff list not initialized");

                Passenger p = _passengers.Find(s => s.Email == email);
                if (p != null)
                {
                    Logger.Instance().Warn($"Staff registration failed: Email {email} already exists");
                    throw new InvalidOperationException($"Email {email} is already registered");
                }

                Passenger passenger = new Passenger()
                {
                    Name = name,
                    Email = email
                };
                passenger.password = password;

                _passengers.Add(passenger);
                Logger.Instance().Info($"New passenger registered: {name} ({email})");
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                Logger.Instance().Error($"Error during passenger registration: {ex.Message}");
                throw new InvalidOperationException("Failed to register passenger", ex);
            }
        }

        public void Save()
        {
            try
            {
                if (_fms == null)
                    throw new InvalidOperationException("FMS not initialized");

                if (_staff == null || _passengers == null)
                    throw new InvalidOperationException("Staff or passenger list not initialized");

                foreach (Staff staff in _staff)
                {
                    if (staff != null)
                        _fms.Save(staff);
                }

                foreach (Passenger passenger in _passengers)
                {
                    if (passenger != null)
                        _fms.Save(passenger);
                }

                Logger.Instance().Info("Staff and Passenger data saved to FMS");
            }
            catch (Exception ex)
            {
                Logger.Instance().Error($"Failed to save data: {ex.Message}");
                throw new InvalidOperationException("Failed to save data to FMS", ex);
            }
        }

        public void DisplayStaffList()
        {
            try
            {
                if (_staff == null)
                    throw new InvalidOperationException("Staff list not initialized");

                if (!_staff.Any())
                {
                    Console.WriteLine("No staff members found.");
                    return;
                }

                foreach (Staff staff in _staff)
                {
                    if (staff != null)
                    {
                        Console.WriteLine(staff.Name);
                        Console.WriteLine(staff.Email);
                        Console.WriteLine(staff.staffCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance().Error($"Error displaying staff list: {ex.Message}");
                throw new InvalidOperationException("Failed to display staff list", ex);
            }
        }
    }
}